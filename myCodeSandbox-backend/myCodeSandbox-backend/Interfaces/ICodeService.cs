using myCodeSandbox_backend.Dtos;
namespace myCodeSandbox_backend.Interfaces;

public interface ICodeService
{
    string CodeExecution(CodeRequestDto requestDto);
}