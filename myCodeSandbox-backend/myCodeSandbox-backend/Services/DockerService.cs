namespace myCodeSandbox_backend.Services;

public class DockerService(IFileService fileService, IContainerService containerService) : IDockerService
{
    public async Task<CodeExecutionResponse> ExecuteCodeAsync(CodeExecutionRequest code)
    {
        try
        {
            // Создаём временный файл с кодом
            var tempFilePath = await fileService.CreateTempFileAsync(code.Language, code.Code);

            // Запускаем контейнер
            CodeExecutionResponse result = await containerService.RunContainerAsync(tempFilePath, code.Language, code.Version);

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
            return new CodeExecutionResponse()
            {
                Output = null,
                Error = $"Ошибка выполнения:\n{ex.Message}",
                Success = false
            };
        }
    }
}