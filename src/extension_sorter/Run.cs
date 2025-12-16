internal static partial class ExtensionSorter
{
    private const string ARG_ERROR =
        "Для работы программы необходимо указать флаг:\n" +
        "  \"-move\" - Для сортировки файлов в указанной директории по расширениям.\n" +
        "  \"-stat\" - ДЛя просмотра информации о количестве файлов с разными расширениями.\n" +
        "И расположение, где и будет осуществляться сортировка файлов по расширениям.\n" +
        "\n" +
        "Внимание!!!\n" +
        "Работа приложения с флагом -sort приведёт к изменению структуры указанного расположения.\n" +
        "Файлы будут перемещены в папки с соответствующими расширениями.\n" +
        "\n" +
        "Для удаления пустых папок после работы программы, необходимо указать флаг \"-rmdir\".";

    // Точка входа:
    // 
    // Идёт проверка на аргументы
    // После чего программа передаёт управление соответствующему разделу
    internal static void Run(string[] args)
    {
        if (IsArgsMistake(args) || IsFlagMistake(args) || IsSecondEmpty(args))
        {
            Console.WriteLine(ARG_ERROR);
            return;
        }

        static bool IsArgsMistake(string[] args) => args.Length < 2 || args.Length > 3;
        static bool IsFlagMistake(string[] args) => args[0] != "-move" && args[0] != "-stat";
        static bool IsSecondEmpty(string[] args) => string.IsNullOrEmpty(args[1]);

        var dir = new DirectoryInfo(args[1]);
        if (!dir.Exists)
        {
            dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, args[1]));

            if (!dir.Exists)
            {
                Console.WriteLine("Указанного расположения не существует!");
                return;
            }
        }

        if (args[0] == "-move")
        {
            var extFiles = new Dictionary<string, List<FileInfo>>();
            GetExtensionFiles(extFiles, dir);
            MoveExtensionFiles(extFiles, dir, args.Contains("-rmdir"));
        }
        else
        {
            var extStat = new Dictionary<string, int>();
            CountExtensions(extStat, dir);
            PrintExtensionStatistic(extStat);
        }
    }
}