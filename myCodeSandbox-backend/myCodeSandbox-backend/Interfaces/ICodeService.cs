namespace myCodeSandbox_backend.Interfaces;

public interface ICodeService
{
    Task<string> CodeExecution(CodeRequestDto requestDto);
}