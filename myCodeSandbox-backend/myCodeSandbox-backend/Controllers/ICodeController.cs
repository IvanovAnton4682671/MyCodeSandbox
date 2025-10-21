namespace myCodeSandbox_backend.Controllers;

public interface ICodeController
{
    Task<IActionResult> CodeExecution([FromBody] CodeExecutionRequest request);
}