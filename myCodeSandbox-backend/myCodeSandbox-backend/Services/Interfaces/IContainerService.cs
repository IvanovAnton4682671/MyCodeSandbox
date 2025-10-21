namespace myCodeSandbox_backend.Services.Interfaces;

public interface IContainerService
{
    Task<CodeExecutionResponse> RunContainerAsync(string filePath, string language, string image);
}