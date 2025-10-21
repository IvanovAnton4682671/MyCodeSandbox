namespace myCodeSandbox_backend.Controllers;

[ApiController]
[Route("api/code")]
public class CodeController(ICodeExecutionService codeExecutionService) : ControllerBase, ICodeController
{
    [HttpPost("execution")]
    public async Task<IActionResult> CodeExecution([FromBody] CodeExecutionRequest request)
    {
        string executionResult = await codeExecutionService.CodeExecutionAsync(request);
        return Ok(executionResult);
    }
}