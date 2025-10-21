namespace myCodeSandbox_backend.Models.Requests;

public class CodeExecutionRequest
{
    public required string Language { get; set; }
    public required string Version { get; set; }
    public required string Code { get; set; }
}