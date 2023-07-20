namespace LearnPractice.Products;

public class ControlProduct : Control
{
    private const string FILE_NAME = "ProductData";
    protected override string Title => "Product";
    protected override Type TypeClass => typeof(ControlProduct);
    protected override string[] _methodsName => new[]{
        "Add New",
        "Delete",
        "Change Info",
        "Find"
    };
    protected override string[] _methods => new[]{
        nameof(AddWindow),
        nameof(DeleteWindow),
        nameof(ChangeInfoWindow),
        nameof(FindWindow)
    };

    private List<Product?> _products = new();
    
    public void AddWindow()
    {
        Console.Clear();
        SConsole.WriteLine("Adding Product", ConsoleColor.Blue);
        Product product = CreateProduct();
        Add(product);
        AwaitKey($"{product.Name} added in list. Press any key that continue", ConsoleColor.Cyan);   
    }

    public void Add(Product product)
    {
        UpdateProducts();
        _products.Add(product);
        SaveData();
    }

    private Product CreateProduct(string name = "",
                                  string description = "",
                                  float price = 0,
                                  ProductCategory category = ProductCategory.None)
    {
        if(string.IsNullOrEmpty(name)){
            Console.Write("Name: ");
            name = Console.ReadLine() ?? string.Empty;
        }

        if(string.IsNullOrEmpty(description)){
            Console.Write("Description: ");
            description = Console.ReadLine() ?? string.Empty;
        }

        if(price == 0){
            Console.Write("Price($): ");

            if(!float.TryParse(Console.ReadLine(), out price)){
                SConsole.WriteLine("Error Number. Price is equals 0", ConsoleColor.Red);
                price = 0;
            }
        }

        if(category == ProductCategory.None)
            category = SelectCategory();

        return new Product(name, description, price, category);
    }

    private ProductCategory SelectCategory()
    {
        Array enums = Enum.GetValues(typeof(ProductCategory));

        for(int i = 0; i < enums.Length; i++){
            ProductCategory value = (ProductCategory)enums.GetValue(i)!;
            if(value == ProductCategory.None)
                continue;
            Console.WriteLine($"{i} {value}");
        }

        if(!int.TryParse(SConsole.ReadLineString("Category"), out int categoryIndex) || categoryIndex < 0 ||
           categoryIndex > enums.Length){
            SConsole.WriteLine("! Error ! You Category is None", ConsoleColor.Red);
            categoryIndex = 0;
        }

        return (ProductCategory)categoryIndex;
    }

    private void ShowAllProducts()
    {
        UpdateProducts();

        if(_products.Count == 0){
            SConsole.WriteLine("Products list is Empty");
            return;
        }

        for(int i = 0; i < _products.Count; i++){
            Product? product = _products[i];
            Console.Write(i + " ");
            product?.ShowDetails();
        }
        
    }

    private void ShowAllProducts(out Product? product)
    {
        ShowAllProducts();
        Console.Write("Select the Product:");

        if(!int.TryParse(Console.ReadLine(), out int index) || index >= _products.Count || index < 0){
            SConsole.WriteLine("Index Error", ConsoleColor.Red);
            product = null;
            return;
        }

        product = _products[index];
    }

    public void DeleteWindow()
    {
        Console.Clear();
        UpdateProducts();
        if(_products.Count == 0)
            SConsole.WriteLine("Product Data is Empty", ConsoleColor.DarkRed);
        SConsole.WriteLine("Delete Product", ConsoleColor.Red);
        ShowAllProducts(out Product? product);

        if(product == null)
            return;

        Delete(product);
        AwaitKey($"{product.Name} deleted in list. Press any key that continue", ConsoleColor.Red);  
    }

    public void Delete(Product? product)
    {
        if(product != null && Contain(product) && _products.Remove(product))
            SaveData();
    }

    public void ChangeInfo(Product? oldProduct, Product? newProduct)
    {
        List<Product?> modifyProducts = new List<Product?>(_products.Count);
        if(modifyProducts == null)
            throw new NullReferenceException();

        foreach(Product? product in _products){
            if(product == oldProduct){
                modifyProducts.Add(newProduct);
                continue;
            }

            modifyProducts.Add(product);
        }

        _products = modifyProducts;
        SaveData();
    }
    
    public void ChangeInfoWindow()
    {
        Console.Clear();
        
        ShowAllProducts(out Product? old);
        if(old == null)
            return;
        
        Console.Clear();
        SConsole.WriteLine($"Change: {old.Name}", ConsoleColor.Blue);
        old.ShowDetails(false, color: ConsoleColor.Cyan);

        switch(ShowCommands(new[]{ "Name", "Description", "Price", "Category", "Change for new Product" },
                            offsetIndex: 1)){
            case 1:
                ChangeInfo(old, CreateProduct(description: old.Description, price: old.Price, category: old.Category));
                break;
            case 2:
                ChangeInfo(old, CreateProduct(name: old.Name, price: old.Price, category: old.Category));
                break;
            case 3:
                ChangeInfo(old, CreateProduct(name: old.Name, description: old.Description, category: old.Category));
                break;
            case 4:
                ChangeInfo(old, CreateProduct(name: old.Name, description: old.Description, price: old.Price));
                break;
            case 5:
                ChangeInfo(old, CreateProduct());
                break;
        }
    }

    public bool Contain(Product? product) => _products.Contains(product);

    public void FindWindow()
    { 
        Console.Clear();
        Console.WriteLine("Please select type find:");

        UpdateProducts();

        Product?[] products = ShowCommands(new[]{ "Name", "Price", "Category" }) switch{
            0 =>
                // Name find
                Find(SConsole.ReadLineString("Name")),
            1 =>
                // Price find
                Find(SConsole.ReadLineFloat("Price")),
            2 =>
                // Category find
                Find(SelectCategory()),
            _ => Array.Empty<Product?>()
        };

        Console.Clear();

        if(products is{ Length: > 0 }){
            foreach(Product? product in products)
                product?.ShowDetails();
            AwaitKey();
        }
    }
    
    public Product?[] Find(string name)
        => _products.Where(p => string.Equals(p?.Name, name, StringComparison.CurrentCultureIgnoreCase)).ToArray();

    public Product?[] Find(float price)
        => _products.Where(p => Math.Abs(p!.Price - price) <= 10 || Math.Abs(p.Price + price) < 10).ToArray();

    public Product?[] Find(ProductCategory category)
        => _products.Where(p => p?.Category == category).ToArray();

    private void UpdateProducts()
        => _products = new List<Product>(SaveAndLoad.Load<Product>(FILE_NAME)!)!;

    private void SaveData() 
        => SaveAndLoad.Save(_products.ToArray(), FILE_NAME);
}