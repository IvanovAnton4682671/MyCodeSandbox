namespace myCodeSandbox_backend.Services;

public class CodeService : ICodeService
{
    public string CodeExecution(CodeRequestDto requestDto)
    {
        return $"Сервер обработал ваш код.\nЯзык: {requestDto.CodeLanguage}\nКод:\n{requestDto.CodeInput}";
    }
}