using System.Reflection;

namespace LearnPractice.Products;

public abstract class Control
{
    protected abstract string Title { get; }
    protected abstract Type TypeClass { get; }
    protected abstract string[] _methodsName { get; }
    protected abstract string[] _methods { get; }

    public void Basic()
    {
        while(true){
            SConsole.WriteLine($"{Title}:", ConsoleColor.Yellow);
            ChangeCommand(out bool exit);
            if(exit)
                return;
        }
    }

    protected void AwaitKey(string message = "", ConsoleColor color = ConsoleColor.White)
    {
        if(!string.IsNullOrEmpty(message))
            SConsole.WriteLine(message,color);
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

    protected int? ShowCommands(string[] commands, bool withIndex = true, int offsetIndex = 0, string field = "")
    {
        for(int i = 0; i < commands.Length; i++){
            string command = commands[i];
            Console.WriteLine(withIndex ? $"{i + offsetIndex} {command}" : $"{command}");
        }

        return SConsole.ReadLineInt(0, commands.Length, field);
    }
}