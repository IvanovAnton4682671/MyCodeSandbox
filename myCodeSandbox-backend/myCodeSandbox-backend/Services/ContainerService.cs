using Version = System.Version;

namespace myCodeSandbox_backend.Services;

public class ContainerService : IContainerService
{
    private readonly DockerClient _dockerClient = new DockerClientConfiguration(
        new Uri(Environment.OSVersion.Platform == PlatformID.Unix ? "unix:///var/run/docker.sock" : "npipe://./pipe/docker_engine")).CreateClient();
    
    private async Task<string> ReadMultiplexedStream(MultiplexedStream multiplexedStream)
    {
        var result = new StringBuilder();
        var buffer = new byte[1024];

        try
        {
            while (true)
            {
                var readResult = await multiplexedStream.ReadOutputAsync(buffer, 0, buffer.Length, CancellationToken.None);
                if (readResult.EOF) break;
                if (readResult.Target == MultiplexedStream.TargetStream.StandardOut || readResult.Target == MultiplexedStream.TargetStream.StandardError)
                {
                    var text = Encoding.UTF8.GetString(buffer, 0, readResult.Count);
                    result.Append(text);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении логов: {ex.Message}");
        }
        
        return result.ToString();
    }
    
    public async Task<CodeExecutionResponse> RunContainerAsync(string filePath, string language, string version)
    {
        // Получаем конфигурацию языка и версии
        if (!CodeExecutionConstants.SupportedLanguages.TryGetValue(language, out var languageConfig))
        {
            return new CodeExecutionResponse()
            {
                Output = null,
                Error = $"Неподдерживаемый язык: {language}",
                Success = false
            };
        }
        if (!languageConfig.SupportedVersions.TryGetValue(version, out var versionConfig))
        {
            return new CodeExecutionResponse()
            {
                Output = null,
                Error = $"Неподдерживаемая версия {version} для языка {language}",
                Success = false
            };
        }
        
        // Имя контейнера
        var containerName = $"codesandbox_{DateTime.Now:yyyyMMdd_HHmmss}";
        
        // Имя файла с кодом
        var fileName = Path.GetFileName(filePath);
        
        // Параметры создания контейнера
        var createParams = new CreateContainerParameters
        {
            Image = versionConfig.ImageName,
            Cmd = new List<string>() { language, fileName },
            Name = containerName,
            HostConfig = new HostConfig()
            {
                Binds = new List<string> { $"{Path.GetDirectoryName(filePath)}:/app:ro" },
                //AutoRemove = true,
                Memory = DockerConstants.MemoryLimit
            },
            WorkingDir = "/app"
        };

        // Заранее зануляем id контейнера
        string containerId = null;

        try
        {
            // Создаём контейнер
            var containerResponse = await _dockerClient.Containers.CreateContainerAsync(createParams);
            containerId = containerResponse.ID;

            // Запускаем контейнер
            var started = await _dockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
            if (!started) throw new Exception($"Не удалось запустить контейнер {containerId}");

            // Ждём завершения выполнения с таймаутом
            var containerTask = _dockerClient.Containers.WaitContainerAsync(containerId);
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(DockerConstants.ExecutionTimeoutSeconds));
            var completedTask = await Task.WhenAny(containerTask, timeoutTask);
            
            // Заранее создаём пустой вывод
            string output = string.Empty;
            
            if (completedTask == timeoutTask)
            {
                await _dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters() { WaitBeforeKillSeconds = 3 });
                
                // Получаем логи таймаута
                var logsAfterTimeout = await _dockerClient.Containers.GetContainerLogsAsync(
                    containerId,
                    false,
                    new ContainerLogsParameters()
                    {
                        ShowStdout = true,
                        ShowStderr = true,
                        Follow = false
                    });
                output = await ReadMultiplexedStream(logsAfterTimeout);
                
                return new CodeExecutionResponse()
                {
                    Output = output,
                    Error = $"Таймаут ожидания ({DockerConstants.ExecutionTimeoutSeconds} секунд)",
                    Success = false
                };
            }

            // Получаем логи до таймаута/удаления контейнера
            var logs = await _dockerClient.Containers.GetContainerLogsAsync(
                containerId,
                false,
                new ContainerLogsParameters()
                {
                    ShowStdout = true,
                    ShowStderr = true,
                    Follow = false
                });
            output = await ReadMultiplexedStream(logs);
            
            return new CodeExecutionResponse()
            {
                Output = output,
                Error = null,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new CodeExecutionResponse()
            {
                Output = null,
                Error = $"Ошибка выполнения:\n{ex.Message}",
                Success = false
            };
        }
        finally
        {
            // Удаляем контейнер
            if (containerId != null)
            {
                try
                {
                    await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters() { Force = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при удалении контейнера: {ex.Message}");
                }
            }
        }
    }
}