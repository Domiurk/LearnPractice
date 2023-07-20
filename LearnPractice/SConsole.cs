namespace LearnPractice;

public abstract class SConsole
{
    public static bool YesOrNo() 
        => ReadLineString("Storage is empty. You want to add Product? (Y/N)").ToLower() == "y";

    public static void InvokeColorAction(Action action, ConsoleColor color)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        action.Invoke();
        Console.ForegroundColor = defaultColor;
    }

    public static void Write(string message, ConsoleColor color = ConsoleColor.White)
        => InvokeColorAction(() => { Console.Write(message); }, color);

    public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
        => InvokeColorAction(() => { Console.WriteLine(message); }, color);

    public static string ReadLineString(string field = "")
    {
        if(!string.IsNullOrEmpty(field))
            Console.Write(field + ":");
        return Console.ReadLine() ?? string.Empty;
    }

    public static int? ReadLineInt(int maxRange, string field = "")
    {
        if(!int.TryParse(ReadLineString(field), out int i) || i > maxRange || i < 0){
            WriteLine("! Error ! No can parse you string to int", ConsoleColor.Red);
            return null;
        }

        return i;
    }

    public static int ReadLineInt(string field = "")
    {
        if(!int.TryParse(ReadLineString(field), out int i))
            WriteLine("! Error ! No can parse you string to int", ConsoleColor.Red);

        return i;
    }

    public static float ReadLineFloat(string field = "")
    {
        if(!float.TryParse(ReadLineString(field), out float i))
            WriteLine("! Error ! No can parse you string to float", ConsoleColor.Red);

        return i;
    }
}