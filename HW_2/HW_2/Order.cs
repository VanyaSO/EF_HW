namespace HW_2;

public class Order
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }

    public List<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();
}