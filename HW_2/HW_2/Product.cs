namespace HW_2;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public static List<Product> GetMoskProducts()
    {
        return new List<Product>
        {
            new Product { Name = "Pen", Price = 1 },
            new Product { Name = "Phone", Price = 10 },
            new Product { Name = "MackBook", Price = 100 },
            new Product { Name = "IceCream", Price = 2 },
        };
    }
}