using HW_2;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    public void EnsurePopulate()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            if (!db.Products.Any())
            {
                db.Products.AddRange(Product.GetMoskProducts());
            }
            db.SaveChanges();
        }
    }
    
    public Product GetProduct(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Products.FirstOrDefault(e => e.Id == id);
        }
    }
    
    //добавления
    public void AddOrder(Order order)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }
    }

    //удаление
    public void RemoveOrder(Order order)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Orders.Remove(order);
            db.SaveChanges();
        }
    }
    
    public Order GetOrder(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Orders.Include(e => e.OrderHistories).ThenInclude(e => e.Product).FirstOrDefault(e => e.Id == id);
        }
    }

    //просмотр
    public void PrintOrder(int id)
    {
        Order order = GetOrder(id);
        if (order != null)
        {
            Console.WriteLine($"{order.FullName} {order.Phone}");
            Console.WriteLine("Products");
            foreach (var orderHistory in order.OrderHistories)
            {
                Product product = GetProduct(orderHistory.ProductId);
                Console.WriteLine($"{product.Name} {product.Price}");
            }
        }
    }
}