namespace LearnPractice.Products;

public class ProductData : Control
{
    private static ProductData _instance = null!;
    public static ProductData Instance => _instance = _instance ?? new ProductData();

    public static readonly string FileName = "ProductData";
    protected override string Title => "Product";
    protected override Type TypeClass => typeof(ProductData);
    protected override string[] _methodsName => new[]{
        "Add New Data",
        "Delete Data",
        "Change Info Data",
        "Find Data"
    };
    protected override string[] _methods => new[]{
        nameof(AddWindow),
        nameof(DeleteWindow),
        nameof(ChangeInfoWindow),
        nameof(FindWindow)
    };

    public static List<Product> Products => new(SaveAndLoad.LoadList<Product>(FileName)!);

    private static List<Product> _products = new();

    private Product? GetProduct()
    {
        Product? product;

        switch(ShowCommands(new[]{ "Select Product", "Find Product", "Add New Product" }, offsetIndex: 1)){
            case 1: // Select Product
                ShowAllProductsData(out product);
                break;
            case 2: // Find Product
                FindWindowWithOut(out product);
                break;
            default:
                product = CreateProduct();
                break;
        }

        return product;
    }

    public void AddWindow(out Product? product)
    {
        Console.Clear();
        SConsole.WriteLine("Adding Product", ConsoleColor.Blue);
        product = CreateProduct();
        AwaitKey($"{product.Name} added in list. Press any key that continue", ConsoleColor.Cyan);
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

        Product result = new (name, description, price, category);
        _products.Add(result);
        SaveData();
        return result;
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

    private void ShowAllProductsData()
    {
        UpdateProducts();

        for(int i = 0; i < _products.Count; i++){
            Product product = _products[i];
            Console.Write(i + " ");
            product.ShowDetails();
        }
    }

    public void ShowAllProductsData(out Product? product)
    {
        ShowAllProductsData();
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

        if(_products.Count == 0){
            SConsole.WriteLine("Product Data is Empty", ConsoleColor.DarkRed);
            return;
        }

        SConsole.WriteLine("Delete Product", ConsoleColor.Red);
        ShowAllProductsData(out Product? product);

        if(product == null)
            return;

        Delete(product);
        AwaitKey($"{product.Name} deleted in list. Press any key that continue", ConsoleColor.Red);
    }

    public void Delete(Product? product)
    {
        if(product != null && Products.Contains(product) && _products.Remove(product))
            SaveData();
    }

    public void ChangeInfo(Product? oldProduct, Product newProduct)
    {
        List<Product> modifyProducts = new List<Product>(_products.Count);
        if(modifyProducts == null)
            throw new NullReferenceException();

        foreach(Product product in _products){
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

        if(_products.Count == 0){
            SConsole.WriteLine("Products Data is Empty", ConsoleColor.DarkRed);
            return;
        }

        ShowAllProductsData(out Product? old);
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

    public void FindWindow()
    {
        FindWindowWithOut(out _, true);
    }

    public void FindWindowWithOut(out Product? product, bool canSelect = false)
    {
        Console.Clear();
        UpdateProducts();
        product = null;

        if(_products.Count == 0){
            SConsole.WriteLine("Product Data is Empty", ConsoleColor.DarkRed);
            return;
        }

        Console.WriteLine("Please select type find:");

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

        if(products.Length > 0 && canSelect){
            for(int i = 0; i < products.Length; i++){
                Product? p = products[i];
                SConsole.WriteLine($"{i}:{p?.Name}");
                p?.ShowDetails(false);
            }

            int? index = SConsole.ReadLineInt(products.Length, "Select");
            if(index != null)
                product = products[(int)index];
            AwaitKey($"You select {product?.Name}. Press Any button for continue");
        }
    }

    public static Product Find(int id)
    {
        UpdateProducts();
        return _products.FirstOrDefault(p => p.ID == id)!;
    }

    public static Product[] Find(string name)
    {
        UpdateProducts();
        return _products.Where(p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase)).ToArray();
    }

    public static Product[] Find(float price)
    {
        UpdateProducts();
        return _products.Where(p => Math.Abs(p.Price - price) <= 10 || Math.Abs(p.Price + price) <= 10).ToArray();
    }

    public static Product[] Find(ProductCategory category)
    {
        UpdateProducts();
        return _products.Where(p => p.Category == category).ToArray();
    }

    private static void UpdateProducts()
        => _products = new List<Product>(SaveAndLoad.LoadList<Product>(FileName)!);

    private static void SaveData()
        => SaveAndLoad.SaveList(_products.ToArray(), FileName);

    public Product GetProductData()
    {
        UpdateProducts();

        Product? product = null;
        int? index = ShowCommands(new[]{ "Create New", "Find", "Show" }, offsetIndex: 1);

        if(index != null){
            product = (int)index switch{
                1 => CreateNew(),
                2 => FindNeedProduct(),
                3 => ShowAllProduct(),
                _ => CreateNew()
            };
        }

        Product CreateNew() => CreateProduct();

        Product? FindNeedProduct()
        {
            int? indexFindCommand = ShowCommands(new[]{ "Name", "Price", "Category" }, offsetIndex: 1);
            Product[] products = Array.Empty<Product>();

            if(indexFindCommand != null)
                products = (int)indexFindCommand switch{
                    1 => Find(SConsole.ReadLineString("Write Name Product")),
                    2 => Find(SConsole.ReadLineFloat("Write price")),
                    3 => Find(SelectCategory()),
                    _ => products
                };

            if(products.Length > 0){
                products.TryGet(out Product? value, "Select");
                if(value != null)
                    return value;
            }

            return null;
        }

        Product? ShowAllProduct()
        {
            UpdateProducts();
            ShowAllProductsData(out product);
            return product;
        }

        if(product == null)
            SConsole.WriteLine("Product is not was Find. Create a new");

        return product ?? CreateNew();
    }
}