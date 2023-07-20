using System.Reflection.Metadata;
using LearnPractice.Products;

namespace LearnPractice.Storage;

public class Storage : Control
{
    private const string FILE_NAME = "Storage";
    protected override string Title => "Storage";
    protected override Type TypeClass => typeof(Storage);
    protected override string[] _methodsName => new[]{
        "Show All Products",
        "Take Product",
        "Put Product",
        "Calculate Price For All Products"
    };
    protected override string[] _methods => new[]{
        nameof(ShowAllProducts),
        nameof(TakeProductWindow),
        nameof(PutProductWindow),
        nameof(CalculatePriceForAllProductsWindow)
    };

    private readonly List<Product> _products = new List<Product>();
    private Dictionary<int, int> _storage = new();
    private Product? _selectedProduct;

    public override void Basic()
    {
        if(_selectedProduct != null)
            SConsole.WriteLine($"You select: {_selectedProduct.Name}");
        base.Basic();
    }

    private bool TryGetProductInfo(int id, out int index, out Product? product)
    {
        product = null;
        index = 0;
        if(_storage.Count == 0){
            SConsole.WriteLine("Storage is Empty", ConsoleColor.DarkCyan);
            return false;
        }

        product = ProductData.Find(id);

        int i = 0;
        foreach(KeyValuePair<int,int> pair in _storage){
            if(pair.Key != id){
                i++;
                continue;
            }

            index = i;
            return true;
        }
        return false;
    }

    private void Show()
    {
        UpdateProducts();

        _products.Clear();
        foreach(KeyValuePair<int, int> pair in _storage){
            if(TryGetProductInfo(pair.Key, out int i, out Product? product) && product != null){
                SConsole.WriteLine($"{product.Name}:{pair.Value}");
                _products.Add(product);
            }
        }
    }

    public void ShowAllProducts()
    {
        Clear();
        Show();
    }
    
    public void TakeProductWindow()
    {
        UpdateProducts();

        if(_storage.Count == 0){
            if(SConsole.ReadLineString("Storage is empty. You want to add Product? (Y/N)").ToLower() == "y")
                _selectedProduct = ProductData.Instance.GetProductData();
        }
        else{
            Show();

            if(_products.ToArray().TryGet(out Product? select, "Select") && select != null){
                _selectedProduct = select;

                if(_storage.ContainsKey(select.ID) && _storage.TryGetValue(select.ID, out int value) && value > 1){
                    _storage[select.ID] = value - 1;
                }
                else{
                    _storage.Remove(select.ID);
                }
            }
        }
    }
    
    public void PutProductWindow()
    {
        Clear();
        PutProductInStorage();
    }

    public void CalculatePriceForAllProductsWindow()
    {
        Clear();
        UpdateProducts();
        // float priceAll = _storage.Sum(pair => GetProductFind(pair.Key).Price * pair.Value);
        // SConsole.WriteLine($"Cost all product on storage: {priceAll}");
    }

    private void PutProductInStorage()
    {
        UpdateProducts();

        if(_selectedProduct == null){
            return;
        }

        if(_storage.TryGetValue(_selectedProduct.ID, out int value))
            _storage[_selectedProduct.ID] = value + 1;
        else
            _storage.TryAdd(_selectedProduct.ID, 1);
        _selectedProduct = null!;

        Save();
    }

    private void Clear()
    {
        Console.Clear();
        if(_selectedProduct != null)
            SConsole.WriteLine($"You select: {_selectedProduct.Name}", ConsoleColor.Cyan);
    }

    private void UpdateProducts()
        => _storage = SaveAndLoad.LoadDictionary<int, int>(FILE_NAME) ?? new Dictionary<int, int>();
    // => _storage = SaveAndLoad.LoadDictionary<string, int>(FILE_NAME) ?? new Dictionary<string, int>();

    private void Save()
        => SaveAndLoad.SaveDictionary(_storage, FILE_NAME);
}