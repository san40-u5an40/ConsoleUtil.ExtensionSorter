internal static partial class ExtensionSorter
{
    // Метод вывода информации об расширениях из коллекции
    private static void PrintExtensionStatistic(Dictionary<string, int> extensions)
    {
        if (extensions.Count == 0)
        {
            Console.WriteLine("В данном расположении нет допустимых файлов для сбора статистики.");
            return;
        }    

        foreach (var extension in extensions.OrderBy(p => p.Key))
            Console.WriteLine($"Расширение \"{extension.Key}\": {extension.Value}");
        Console.WriteLine("    Всего: " + extensions.Sum(p => p.Value));
    }
}