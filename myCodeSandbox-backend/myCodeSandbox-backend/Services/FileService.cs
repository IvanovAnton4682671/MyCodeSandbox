namespace myCodeSandbox_backend.Services;

public class FileService : IFileService
{
    public async Task<string> CreateTempFileAsync(string language, string code)
    {
        // Поднимаемся на 2 уровня вверх (в MyCodeSandbox)
        DirectoryInfo currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        DirectoryInfo targetDir = currentDir.Parent?.Parent ?? throw new DirectoryNotFoundException("Не удалось подняться на 2 уровня вверх");
        
        // Путь к папке докера
        string tempDockerDirPath = Path.Combine(targetDir.FullName, DockerConstants.TempDockerDir);
        
        // Проверяем/создаём папку докера
        if (!Directory.Exists(tempDockerDirPath)) Directory.CreateDirectory(tempDockerDirPath);
        
        // Проверяем/создаём подпапку языка
        string subfolderPath = Path.Combine(tempDockerDirPath, CodeExecutionConstants.SupportedLanguages[language].Folder);
        if (!Directory.Exists(subfolderPath)) Directory.CreateDirectory(subfolderPath);
        
        // Полный путь к целевой папке
        string finalPath = Path.Combine(tempDockerDirPath, subfolderPath);
        
        // Создаём уникальное имя файла
        string fileName = $"code_{DateTime.Now:yyyyMMdd_HHmmss}.{CodeExecutionConstants.SupportedLanguages[language].FileExtension}";
        string fullFilePath = Path.Combine(finalPath, fileName);
        
        // Записываем файл
        await File.WriteAllTextAsync(fullFilePath, code, Encoding.UTF8);
        return fullFilePath;
    }
}