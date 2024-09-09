namespace HW_dop_1;

public class StoreProduct
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int StoreId { get; set; }
    public int QuantityInStock { get; set; }
    
    public Product Product { get; set; }
    public Store Store { get; set; }
}