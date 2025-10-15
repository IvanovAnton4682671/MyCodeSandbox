namespace myCodeSandbox_backend.Interfaces;

public interface IDockerService
{
    string CreateTempFileAsync(string language, string code);
    //Task<CodeResponseDto> ExecuteCodeAsync(CodeRequestDto requestDto);
}