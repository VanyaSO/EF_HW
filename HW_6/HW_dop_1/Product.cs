namespace HW_dop_1;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductType Type { get; set; }
    public double Price { get; set; }
    public List<Store> Stores { get; set; }
    public List<StoreProduct> StoreProducts { get; set; }
}