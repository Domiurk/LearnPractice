namespace LearnPractice;

public static class Extensions
{
    public static bool TryGet<T>(this T[] items,out T? item, string field = "")
    {
        int? index = SConsole.ReadLineInt(items.Length, field);
        bool result = index < items.Length && index >= 0;

        if(index != null && result){
            item = items[(int)index];
            return true;
        }

        item = default;
        return false;
    }
}