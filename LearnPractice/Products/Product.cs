namespace LearnPractice.ControlProducts;

public class Product
{
    public string Name { get;}
    public string Description { get;}
    public float Price { get;  }
    public ProductCategory Category { get;}

    public Product(string name, string description, float price, ProductCategory category)
    {
        Name = name;
        Description = description;
        Price = price;
        Category = category;
    }
}

public enum ProductCategory
{
    Electronics,
    Clothing,
    Books
}
