using myCodeSandbox_backend.Dtos;
using myCodeSandbox_backend.Interfaces;
namespace myCodeSandbox_backend.Services;

public class CodeService : ICodeService
{
    public string CodeExecution(CodeRequestDto requestDto)
    {
        if (requestDto.CodeInput.Trim().Length == 0)
        {
            return string.Empty;
        }
        else
        {
            return "Сервер обработал ваш код:\n" + requestDto.CodeInput;
        }
    }
}