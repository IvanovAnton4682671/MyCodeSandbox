namespace myCodeSandbox_backend.Services.Interfaces;

public interface ICodeExecutionService
{
    Task<string> CodeExecutionAsync(CodeExecutionRequest code);
}