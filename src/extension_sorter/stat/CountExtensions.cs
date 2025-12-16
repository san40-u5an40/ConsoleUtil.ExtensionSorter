internal static partial class ExtensionSorter
{
    // Метод подсчёта файлов с разными расширениями
    // 
    // Рекурсивно перебираются все папки в расположении
    // Если были найдены файлы, в коллекции увеличивается счётчик соответствующего расширения
    private static void CountExtensions(Dictionary<string, int> extStat, DirectoryInfo dir)
    {
        try
        {
            var dirs = dir
                .GetDirectories()
                .Where(p => p.Name != "System Volume Information" && p.Name != "$Recycle.Bin" && p.Name != "Documents and Settings");
            
            foreach (var dirInfo in dirs)
                CountExtensions(extStat, dirInfo);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Нет доступа к расположению: \"{dir.FullName}\"");
        }

        var files = dir.GetFiles();
        foreach (var file in files)
        {
            string ext = file.Extension.ToLower();

            if (extStat.ContainsKey(ext))
                extStat[ext]++;
            else
                extStat.Add(ext, 1);
        }
            
    }
}