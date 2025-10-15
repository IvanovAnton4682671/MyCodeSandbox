namespace myCodeSandbox_backend.Services;

public class DockerService : IDockerService
{
    private readonly List<string> _folderNames = ["tempDockerDir", "python", "java", "cpp"];
    private readonly Dictionary<string, string> _languageExtensions = new Dictionary<string, string>()
    {
        { "python", ".py" },
        { "java", ".java" },
        { "cpp", ".cpp" }
    };

    public string CreateTempFileAsync(string language, string code)
    {
        // Поднимаемся на 3 уровня вверх (в MyCodeSandbox)
        DirectoryInfo currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        DirectoryInfo targetDir = currentDir.Parent?.Parent ?? throw new DirectoryNotFoundException("Не удалось подняться на 3 уровня вверх");
        
        // Путь к папке докера
        string tempDockerDirPath = Path.Combine(targetDir.FullName, _folderNames[0]);
        
        // Проверяем/создаём папку докера
        if (!Directory.Exists(tempDockerDirPath)) Directory.CreateDirectory(tempDockerDirPath);
        
        // Проверяем/создаём подпапки
        string[] subfolders = [_folderNames[1], _folderNames[2], _folderNames[3]];
        foreach (string folder in subfolders)
        {
            string path = Path.Combine(tempDockerDirPath, folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        
        // Определяем целевую папку по расширению
        string targetSubfolder = language;
        
        // Полный путь к целевой папке
        string finalPath = Path.Combine(tempDockerDirPath, targetSubfolder);
        
        // Создаём уникальное имя файла
        string fileName = $"code_{DateTime.Now:yyyyMMdd_HHmmss}{_languageExtensions[language]}";
        string fullFilePath = Path.Combine(finalPath, fileName);
        
        // Записываем файл
        File.WriteAllText(fullFilePath, code);
        return fullFilePath;
    }
}