public abstract class SConsole
{
    public static void Write(string message, ConsoleColor color = ConsoleColor.White)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = defaultColor;
    }
    
    public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = defaultColor;
    }
}