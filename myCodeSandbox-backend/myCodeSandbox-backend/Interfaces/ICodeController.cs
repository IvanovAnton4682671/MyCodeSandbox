using Microsoft.AspNetCore.Mvc;
using myCodeSandbox_backend.Dtos;
namespace myCodeSandbox_backend.Interfaces;

public interface ICodeController
{
    Task<IActionResult> CodeExecution(CodeRequestDto requestDto);
}