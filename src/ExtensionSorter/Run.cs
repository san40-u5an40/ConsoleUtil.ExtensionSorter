internal static partial class ExtensionSorter
{
    private const string HELP =
        "Для работы программы необходимо указать команду\n" +
        "  \"move\" - Для сортировки файлов в указанной директории по расширениям.\n" +
        "  \"stat\" - Для просмотра информации о количестве файлов с разными расширениями.\n" +
        "И расположение, где и будет осуществляться сортировка файлов по расширениям.\n" +
        "\n" +
        "Внимание!!!\n" +
        "Работа приложения с командой \"move\" приведёт к изменению структуры указанного расположения.\n" +
        "Файлы будут перемещены в папки с соответствующими расширениями.\n" +
        "\n" +
        "Для удаления пустых папок после работы программы, необходимо указать флаг \"/rmdir\" после обязательных аргументов.";

    // Точка входа:
    // 
    // Идёт проверка на аргументы
    // После чего программа передаёт управление соответствующему разделу
    internal static void Run(string[] args)
    {
        var argumentsBuilder = new ArgumentsBuilder()
            .AddSection("stat")
            .AddSection("move");

        argumentsBuilder["stat"]
            .AddDirectory();

        argumentsBuilder["move"]
            .AddDirectory()
            .AddOptional("/rmdir");

        var argumentsBuilderResult = argumentsBuilder.Build();

        if (!argumentsBuilderResult.IsValid)
        {
            string errorMessage =
                argumentsBuilderResult.Error.Type == FailureArgumentsType.BindingMistake ?
                argumentsBuilderResult.Error.Message :
                HELP;

            Console.WriteLine(errorMessage);
            return;
        }

        var arguments = argumentsBuilderResult.Value;
        var dir = new DirectoryInfo(arguments[0]);

        switch (arguments.Section)
        {
            case "stat":
                var extStat = new Dictionary<string, int>();
                CountExtensions(extStat, dir);
                PrintExtensionStatistic(extStat);
                return;

            case "move":
                var extFiles = new Dictionary<string, List<FileInfo>>();
                GetExtensionFiles(extFiles, dir);
                MoveExtensionFiles(extFiles, dir, arguments.Contains("/rmdir"));
                return;
        }
    }
}