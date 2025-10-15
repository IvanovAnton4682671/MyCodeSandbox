namespace myCodeSandbox_backend.Interfaces;

public interface IDockerService
{
    Task<CodeResponseDto> ExecuteCodeAsync(CodeRequestDto requestDto);
}