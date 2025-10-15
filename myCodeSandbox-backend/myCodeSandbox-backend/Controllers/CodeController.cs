namespace myCodeSandbox_backend.Controllers;

[ApiController]
[Route("api/code")]
public class CodeController(IDockerService dockerService) : ControllerBase, ICodeController
{
    [HttpPost("execution")]
    public async Task<IActionResult> CodeExecution(CodeRequestDto requestDto)
    {
        string resFilePath = dockerService.CreateTempFileAsync(requestDto.CodeLanguage, requestDto.CodeInput);
        return Ok(resFilePath);
    }
}