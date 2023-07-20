using System.Text;
using Newtonsoft.Json;

namespace LearnPractice;

public static class SaveAndLoad
{
    private const string FILE_PATH = @"D:\Documents\C# Projects\LearnPractice\LearnPractice\";
    private const string FILE_EXTENSION = ".json";
    private static readonly Encoding _encoding = Encoding.UTF8;

    public static T?[] LoadList<T>(string fileName)
    {
        string fullPath = FILE_PATH + fileName + FILE_EXTENSION;

        if(File.Exists(fullPath)){
            string json = File.ReadAllText(fullPath, _encoding);
            return JsonConvert.DeserializeObject<T[]>(json) ?? Array.Empty<T>();
        }

        return Array.Empty<T>();
    }

    public static void SaveList<T>(T[] product, string fileName)
    {
        string json = JsonConvert.SerializeObject(product, Formatting.Indented);
        File.WriteAllText(FILE_PATH + fileName + FILE_EXTENSION, json, _encoding);
    }

    public static Dictionary<TKey, TValue> LoadDictionary<TKey, TValue>(string fileName) where TKey : notnull
    {
        string fullPath = FILE_PATH + fileName + FILE_EXTENSION;

        if(File.Exists(fullPath)){
            string json = File.ReadAllText(fullPath, _encoding);
            return JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json) ?? new Dictionary<TKey, TValue>();
        }

        return new Dictionary<TKey, TValue>();
    }

    public static void SaveDictionary<TKey, TValue>(Dictionary<TKey, TValue> product, string fileName)
        where TKey : notnull
    {
        string json = JsonConvert.SerializeObject(product, Formatting.Indented);
        File.WriteAllText(FILE_PATH + fileName + FILE_EXTENSION, json);
    }
}