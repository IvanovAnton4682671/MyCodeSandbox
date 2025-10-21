namespace myCodeSandbox_backend.Constants;

public class VersionConfig
{
    public required string ImageName { get; set; }
}

public class LanguageConfig
{
    public required string Language { get; set; }
    public required Dictionary<string, VersionConfig> SupportedVersions { get; set; }
    public required string FileExtension { get; set; }
    public required string BaseCommand { get; set; }
    public required string Folder { get; set; }
}

public static class CodeExecutionConstants
{
    public static readonly Dictionary<string, LanguageConfig> SupportedLanguages = new Dictionary<string, LanguageConfig>()
    {
        ["python"] = new LanguageConfig()
        {
            Language = "python",
            SupportedVersions = new Dictionary<string, VersionConfig>()
            {
                ["3.9-slim"] = new VersionConfig() { ImageName = "python:3.9-slim" }
            },
            FileExtension = "py",
            BaseCommand = "python",
            Folder = "python"
        }
    };
}