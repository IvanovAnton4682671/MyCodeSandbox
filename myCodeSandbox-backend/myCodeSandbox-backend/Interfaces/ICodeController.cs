namespace myCodeSandbox_backend.Interfaces;

public interface ICodeController
{
    Task<IActionResult> CodeExecution(CodeRequestDto requestDto);
}