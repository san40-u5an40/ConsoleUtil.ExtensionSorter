internal static partial class ExtensionSorter
{
    // Метод перемещения всех найденных файлов по подпапкам в соответствии с их расширением
    // 
    // Перебираются все найденные расширения
    // И для каждого создаётся папка, куда будут перемещены соответствующие файлы
    // Далее эти файлы перемещаются
    // Если возникает ошибка, выводится соответствующее уведомление
    // 
    // После перемещения всех файлов, при необходимости, вызывается рекурсивная функция удаления пустых папок
    // Ну и в конце показывается уведомление о завершении работы программы
    private static void MoveExtensionFiles(Dictionary<string, List<FileInfo>> extensions, DirectoryInfo dir, bool isDeleteEmptyDirectories)
    {
        foreach (var extension in extensions)
        {
            string pathExt = Path.Combine(dir.FullName, extension.Key);
            Directory.CreateDirectory(pathExt);

            foreach (var file in extension.Value)
            {
                try
                {
                    file.MoveTo(Path.Combine(pathExt, file.Name));
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Не получилось переместить файл \"{file.FullName}\"\n" +
                        $"Ошибка: {ex.Message}\n");
                }
            }
        }

        if (isDeleteEmptyDirectories)
            DeleteEmptyDirectories(dir);

        Console.WriteLine("Сортировка по расширениям завершена!");

        // Локальная функция для рекурсивного удаления пустых папок
        // 
        // Она перебирает все папки внутри
        // И если в них нет файлов, удаляет их
        // Вроде всё логично ))
        static void DeleteEmptyDirectories(DirectoryInfo dir)
        {
            try
            {
                var dirs = dir
                    .GetDirectories()
                    .Where(p => p.Name != "System Volume Information" && p.Name != "$Recycle.Bin" && p.Name != "Documents and Settings");

                foreach (var dirInfo in dirs)
                    DeleteEmptyDirectories(dirInfo);

                if (dir.GetFiles().Length == 0)
                    dir.Delete();
            }
            catch { /* Уведомление об отсутствии доступа к расположению было показано в методе GetExtensionFiles */ }
        }
    }
}