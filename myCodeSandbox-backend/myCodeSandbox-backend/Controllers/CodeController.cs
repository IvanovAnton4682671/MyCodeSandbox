namespace myCodeSandbox_backend.Controllers;

[ApiController]
[Route("api/code")]
public class CodeController(ICodeService codeService) : ControllerBase, ICodeController
{
    [HttpPost("execution")]
    public async Task<IActionResult> CodeExecution([FromBody] CodeRequestDto requestDto)
    {
        var executionResult = await codeService.CodeExecution(requestDto);
        return Ok(executionResult);
    }
}