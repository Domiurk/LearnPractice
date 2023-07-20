using LearnPractice.Products;
using LearnPractice.Users;

namespace LearnPractice;

public interface IControllable
{
    void Basic();
}

public interface IControllableProduct : IControllable
{
    
    void Add(Product? product);
    void Delete(Product? product);
    void ChangeInfo(Product? oldP, Product? newP);
    bool Contain(Product? product);
    Product? Find(string name);
    Product? Find(float price);
    Product? Find(ProductCategory category);
}

public interface IControllableStorage : IControllable
{
    int ShowProductsInStorage();
    void PickProduct(Product product);
    void DropProduct(Product product);
    float CalculatePriceForAllProducts();
}

public interface IControllableOrder : IControllable
{
    void CreateNewOrder();
    void AddProductInOrder(Product product);
    void ChangeProductsInOrder(Product product);
    void DeleteProductInOrder(Product product);
    void DoneOrder();
}

public interface IControllableUsers : IControllable
{
    TypeUser Role { get; }
    void RegisterNewUser();
    void AuthenticationUser(Product product);
}