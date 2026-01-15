internal static partial class ExtensionSorter
{
    // Метод получения коллекции файлов в соответствии с их расширением
    // 
    // Рекурсивно перебираются все подпапки в указанной директории
    // И файлы из них добавляются в библиотеку
    private static void GetExtensionFiles(Dictionary<string, List<FileInfo>> extFilesDictionary, DirectoryInfo dir)
    {
        try
        {
            var dirs = dir
                .GetDirectories()
                .Where(p => p.Name != "System Volume Information" && p.Name != "$Recycle.Bin" && p.Name != "Documents and Settings");

            foreach (var dirInfo in dirs)
                GetExtensionFiles(extFilesDictionary, dirInfo);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Нет доступа к расположению: \"{dir.FullName}\"");
        }

        var directoryFiles = dir.GetFiles();
        foreach (var file in directoryFiles)
        {
            string ext = file.Extension.ToLower();

            if (extFilesDictionary.TryGetValue(ext, out List<FileInfo>? extFiles))
                extFiles.Add(file);
            else
                extFilesDictionary[ext] = [file];
        }
    }
}