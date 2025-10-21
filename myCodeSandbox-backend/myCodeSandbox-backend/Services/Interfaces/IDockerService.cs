namespace myCodeSandbox_backend.Services.Interfaces;

public interface IDockerService
{
    Task<CodeExecutionResponse> ExecuteCodeAsync(CodeExecutionRequest code);
}