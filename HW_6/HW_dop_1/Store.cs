namespace HW_dop_1;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
    public List<Supplier> Suppliers { get; set; }
    public List<Product> Products { get; set; }
    public List<StoreProduct> StoreProducts { get; set; }
    
    public City City { get; set; }
}