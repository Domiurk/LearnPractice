using System.Reflection;
using LearnPractice.Products;

namespace LearnPractice;

public abstract class Control
{
    protected abstract string Title { get; }
    protected abstract Type TypeClass { get; }
    protected abstract string[] _methodsName { get; }
    protected abstract string[] _methods { get; }

    public virtual void Basic()
    {
        SConsole.WriteLine($"{Title}:", ConsoleColor.Yellow);
        ChangeCommand(out bool exit);
        if(exit)
            return;
        Basic();
    }

    protected void AwaitKey(string message = "", ConsoleColor color = ConsoleColor.White)
    {
        if(!string.IsNullOrEmpty(message))
            SConsole.WriteLine(message, color);
        Console.ReadKey();
        Console.Clear();
    }

    private void ChangeCommand(out bool exit)
    {
        string[] methodsName = _methodsName;

        if(_methods.Length > methodsName.Length){
            int size = _methods.Length - methodsName.Length;
            Array.Resize(ref methodsName, _methods.Length);

            for(int i = 0; i < size; i++)
                methodsName[methodsName.Length - 1 - i] = _methods[methodsName.Length - 1 - i];
        }

        if(_methods.Length < methodsName.Length){
            Array.Resize(ref methodsName, _methods.Length);
        }

        string[] commands = new string[methodsName.Length + 1];

        for(int i = 0; i < methodsName.Length; i++){
            commands[i] = methodsName[i];
        }

        commands[methodsName.Length] = "Exit";
        int? index = ShowCommands(commands, field: "Select");

        if(index == null || index == commands.Length || index == commands.Length - 1){
            exit = true;
            return;
        }

        exit = false;

        MethodInfo? method = TypeClass.GetMethod(_methods[(Index)index]);
        method?.Invoke(this, null);
    }

    protected static int? ShowCommands(string[] commands, int offsetIndex = 0, bool withIndex = true, string field = "")
    {
        for(int i = 0; i < commands.Length; i++){
            string command = commands[i];
            Console.WriteLine(withIndex ? $"{i + offsetIndex} {command}" : $"{command}");
        }

        return SConsole.ReadLineInt(commands.Length, field);
    }

    public static void Show<T>(IList<T?> items, bool visibleIndex, out T? selectItem) where T : class, IName
    {
        foreach(T? item in items){
            SConsole.WriteLine(visibleIndex ? $"{items.IndexOf(item)}|{item?.Name}" : $"{item?.Name}");
        }

        int? index = SConsole.ReadLineInt(items.Count, "Select");

        if(index != null){
            selectItem = items[(int)index];
        }
        else{
            SConsole.WriteLine("! Error ! Try parse is failed", ConsoleColor.DarkRed);
            selectItem = null;
            return;
        }
    }

    public static void Show(IDictionary<Product, int> items, bool visibleIndex, out Product? selectItem)
    {
        List<Product> products = new List<Product>();
        int count = 0;

        foreach(KeyValuePair<Product, int> pair in items){
            products.Add(pair.Key);
            SConsole.WriteLine(visibleIndex ? $"{count}|{pair.Key.Name}" : $"{pair.Value}");
        }

        int? index = SConsole.ReadLineInt(items.Count, "Select");

        if(index != null){
            selectItem = products[(int)index];
        }
        else{
            SConsole.WriteLine("! Error ! Try parse is failed", ConsoleColor.DarkRed);
            selectItem = null;
            return;
        }
    }

    public static void Show<T>(IList<T?> items, bool visibleIndex) where T : class, IName
        => Show(items, visibleIndex, out T? _);
}