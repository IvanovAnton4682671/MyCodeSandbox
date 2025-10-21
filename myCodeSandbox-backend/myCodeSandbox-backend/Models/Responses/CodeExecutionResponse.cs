namespace myCodeSandbox_backend.Models.Responses;

public class CodeExecutionResponse
{
    public string? Output { get; set; }
    public string? Error { get; set; }
    public bool Success { get; set; }
}