
namespace LearnPractice.Products;

public sealed class Product : IName
{
    public string Name { get; }
    public string Description { get; }
    public float Price { get; } 
    public ProductCategory Category { get; }
    public int ID { get; }

    public Product(string name, string description, float price, ProductCategory category)
    {
        Name = name;
        Description = description;
        Price = price;
        Category = category;
        ID = this.GetHashCode();
    }

    public void ShowDetails(bool isName = true, bool isDescription = true, bool isPrice = true, bool isCategory = true, ConsoleColor color = ConsoleColor.White)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        if(isName)
            Console.WriteLine(Name);
        if(isDescription)
            Console.WriteLine($"\t Description:{Description}");
        if(isPrice)
            Console.WriteLine($"\t Price:{Price}");
        if(isCategory)
            Console.WriteLine($"\t Category:{Category}");
        Console.ForegroundColor = defaultColor;
    }
}

public interface IName
{
    string Name { get; }
}

public enum ProductCategory
{
    None = 0,
    Electronics = 1,
    Clothing = 2,
    Books = 3
}