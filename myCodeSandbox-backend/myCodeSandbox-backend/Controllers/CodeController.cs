using myCodeSandbox_backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using myCodeSandbox_backend.Interfaces;
using myCodeSandbox_backend.Services;

namespace myCodeSandbox_backend.Controllers;

[ApiController]
[Route("api/code")]
public class CodeController : ControllerBase, ICodeController
{
    [HttpPost("execution")]
    public async Task<IActionResult> CodeExecution(CodeRequestDto requestDto)
    {
        CodeService codeService = new CodeService();
        string executionResult = codeService.CodeExecution(requestDto);
        return Ok(executionResult);
    }
}