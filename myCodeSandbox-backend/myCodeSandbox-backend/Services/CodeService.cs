namespace myCodeSandbox_backend.Services;

public class CodeService(IDockerService dockerService) : ICodeService
{
    public async Task<string> CodeExecution(CodeRequestDto requestDto)
    {
        var result = await dockerService.ExecuteCodeAsync(requestDto);
        if (!result.Success) return $"Ошибка:\n{result.Error}";
        return $"Результат выполнения:\n{result.CodeResult}";
    }
}