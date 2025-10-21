namespace myCodeSandbox_backend.Services;

public class CodeExecutionService(IDockerService dockerService) : ICodeExecutionService
{
    public async Task<string> CodeExecutionAsync(CodeExecutionRequest code)
    {
        CodeExecutionResponse result = await dockerService.ExecuteCodeAsync(code);
        return result.Success ? $"Результат выполнения:\n{result.Output}" : $"Ошибка:\n{result.Error}";
    }
}