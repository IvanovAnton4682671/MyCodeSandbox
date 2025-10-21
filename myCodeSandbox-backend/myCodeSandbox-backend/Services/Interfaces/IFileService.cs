namespace myCodeSandbox_backend.Services.Interfaces;

public interface IFileService
{
    Task<string> CreateTempFileAsync(string language, string code);
}