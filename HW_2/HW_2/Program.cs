namespace HW_2;

class Program
{
    static void Main(string[] args)
    {
        OrderService orderService = new OrderService();
        orderService.EnsurePopulate();

        orderService.AddOrder(
            new Order
            {
                FullName = "Will Smith",
                Phone = "000000000",
                OrderHistories =
                {
                    new OrderHistory
                    {
                        Quantity = 1,
                        ProductId = 2
                    }
                }
            }
        );
        
        orderService.AddOrder(
            new Order { 
                FullName = "Ivan Ushachov", 
                Phone = "111111111", 
                OrderHistories =
                {
                    new OrderHistory
                    {
                        Quantity = 1,
                        ProductId = 3
                    },
                    new OrderHistory
                    {
                        Quantity = 2,
                        ProductId = 4
                    }
                }
            }
        );
        
        orderService.RemoveOrder(orderService.GetOrder(5));

        orderService.PrintOrder(6);
    }
}