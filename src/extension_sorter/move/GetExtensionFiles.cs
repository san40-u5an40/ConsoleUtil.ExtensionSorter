internal static partial class ExtensionSorter
{
    // Метод получения коллекции файлов в соответствии с их расширением
    // 
    // Рекурсивно перебираются все подпапки в указанной директории
    // И файлы из них добавляются в библиотеку
    private static void GetExtensionFiles(Dictionary<string, List<FileInfo>> extFiles, DirectoryInfo dir)
    {
        try
        {
            var dirs = dir
                .GetDirectories()
                .Where(p => p.Name != "System Volume Information" && p.Name != "$Recycle.Bin" && p.Name != "Documents and Settings");

            foreach (var dirInfo in dirs)
                GetExtensionFiles(extFiles, dirInfo);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Нет доступа к расположению: \"{dir.FullName}\"");
        }

        var files = dir.GetFiles();
        foreach (var file in files)
        {
            string ext = file.Extension.ToLower();

            if (extFiles.ContainsKey(ext))
                extFiles[ext].Add(file);
            else
                extFiles[ext] = new List<FileInfo> { file };
        }
    }
}