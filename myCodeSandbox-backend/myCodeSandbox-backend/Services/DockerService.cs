using System.Diagnostics;

namespace myCodeSandbox_backend.Services;

public class DockerService : IDockerService
{
    private readonly DockerClient _dockerClient = new DockerClientConfiguration(
        new Uri(Environment.OSVersion.Platform == PlatformID.Unix ? "unix:///var/run/docker.sock" : "npipe://./pipe/docker_engine")).CreateClient();
    private readonly List<string> _folderNames = ["tempDockerDir", "python", "java", "cpp"];
    private readonly Dictionary<string, string> _languageExtensions = new Dictionary<string, string>()
    {
        { "python", "py" },
        { "java", "java" },
        { "cpp", "cpp" }
    };

    // Вспомогательная структура для формирования образа
    private struct ImageConfig
    {
        public required string ImageName { get; set; }
        public required string FileExtension { get; set; }
        public required Func<string, List<string>> Command { get; set; }

        public List<string> GetCommand(string fileName) => Command(fileName);
    }

    // Подготовка данных образа
    private ImageConfig GetImageConfig(string language)
    {
        return language switch
        {
            "python" => new ImageConfig
            {
                ImageName = "python:3.9-slim",
                FileExtension = _languageExtensions[language],
                Command = (fileName) => new List<string> { "python", fileName }
            },
            "java" => new ImageConfig
            {
                ImageName = "openjdk:17-jdk-slim",
                FileExtension = _languageExtensions[language],
                Command = (fileName) => new List<string> { "sh", "-c", $"javac {fileName} && java {Path.GetFileNameWithoutExtension(fileName)}" }
            },
            _ => throw new ArgumentException($"Неподдерживаемый язык: {language}")
        };
    }
    
    // Создание файла с кодом
    private async Task<string> CreateTempFileAsync(string language, string code)
    {
        // Поднимаемся на 2 уровня вверх (в MyCodeSandbox)
        DirectoryInfo currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        DirectoryInfo targetDir = currentDir.Parent?.Parent ?? throw new DirectoryNotFoundException("Не удалось подняться на 2 уровня вверх");
        
        // Путь к папке докера
        string tempDockerDirPath = Path.Combine(targetDir.FullName, _folderNames[0]);
        
        // Проверяем/создаём папку докера
        if (!Directory.Exists(tempDockerDirPath)) Directory.CreateDirectory(tempDockerDirPath);
        
        // Проверяем/создаём подпапки
        string[] subfolders = [_folderNames[1], _folderNames[2], _folderNames[3]];
        foreach (string folder in subfolders)
        {
            string path = Path.Combine(tempDockerDirPath, folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        
        // Определяем целевую папку по расширению
        string targetSubfolder = language;
        
        // Полный путь к целевой папке
        string finalPath = Path.Combine(tempDockerDirPath, targetSubfolder);
        
        // Создаём уникальное имя файла
        string fileName = $"code_{DateTime.Now:yyyyMMdd_HHmmss}.{_languageExtensions[language]}";
        string fullFilePath = Path.Combine(finalPath, fileName);
        
        // Записываем файл
        await File.WriteAllTextAsync(fullFilePath, code, Encoding.UTF8);
        return fullFilePath;
    }

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
    
    private async Task<CodeResponseDto> RunContainerAsync(string filePath, ImageConfig imageConfig)
    {
        // Имя контейнера
        var containerName = $"codesandbox_{DateTime.Now:yyyyMMdd_HHmmss}";
        
        // Имя файла с кодом
        var fileName = Path.GetFileName(filePath);
        
        // Параметры создания контейнера
        var createParams = new CreateContainerParameters
        {
            Image = imageConfig.ImageName,
            Cmd = imageConfig.GetCommand(Path.GetFileName(filePath)),
            Name = containerName,
            HostConfig = new HostConfig()
            {
                Binds = new List<string> { $"{Path.GetDirectoryName(filePath)}:/app:ro" },
                //AutoRemove = true,
                Memory = 1024 * 1024 * 256
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
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30));
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
                
                return new CodeResponseDto()
                {
                    CodeResult = output,
                    Error = "Таймаут ожидания (30 секунд)",
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
            
            return new CodeResponseDto()
            {
                CodeResult = output,
                Error = null,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new CodeResponseDto()
            {
                CodeResult = null,
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

    public async Task<CodeResponseDto> ExecuteCodeAsync(CodeRequestDto requestDto)
    {
        try
        {
            // Сопоставляем язык с Docker-образом
            var imageConfig = GetImageConfig(requestDto.CodeLanguage);

            // Создаём временный файл с кодом
            var tempFilePath = await CreateTempFileAsync(requestDto.CodeLanguage, requestDto.CodeInput);

            // Запускаем контейнер
            CodeResponseDto result = await RunContainerAsync(tempFilePath, imageConfig);

            // Удаляем временный файл
            try
            {
                File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении временного файла: {ex.Message}");
            }

            return result;
        }
        catch (Exception ex)
        {
            return new CodeResponseDto()
            {
                CodeResult = null,
                Error = $"Ошибка выполнения:\n{ex.Message}",
                Success = false
            };
        }
    }
}