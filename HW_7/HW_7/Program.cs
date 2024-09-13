using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace HW_7;

// Создайте базу данных со следующими таблицами:
//
// Clients: Содержит информацию о клиентах, включая их имя, электронную почту и адрес.
public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public List<Order> Orders { get; set; }
}

// Products: Содержит информацию о товарах, включая их название и стоимость.
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
}

// Orders: Содержит информацию о заказах, включая покупателя, дату заказа, адрес доставки.
public class Order
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DeliveryAddress { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }

    public Client Client { get; set; }
}

// OrderDetails: Содержит информацию о товарах в каждом заказе, включая идентификатор товара, идентификатор заказа и количество товара.
public class OrderDetail
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }

    public Product Product { get; set; }
    public Order Order { get; set; }
}

// Вам необходимо написать один запрос LINQ to Entities, который извлекает список клиентов вместе со следующей информацией:
//
// Общее количество заказов для каждого клиента.
// Общая сумма, потраченная каждым клиентов.
// Самый дорогой товар, купленный каждым клиентом.
//
// Запрос должен вернуть список объектов со следующими свойствами:
// Имя клиента
// Электронная почта
// Адрес
// Общее количество заказов
// Общая потраченная сумма
// Название самого дорогого приобретенного товара

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            
            db.Clients.AddRange(new List<Client>
            {
                new Client { Name = "Иван Иванов", Email = "ivanov@mail.com", Address = "ул. Ленина, 10" },
                new Client { Name = "Мария Петрова", Email = "petrova@mail.com", Address = "пр. Мира, 25" },
                new Client { Name = "Алексей Смирнов", Email = "smirnov@mail.com", Address = "ул. Советская, 12" },
                new Client { Name = "Ольга Васильева", Email = "vasilieva@mail.com", Address = "ул. Гагарина, 5" }
            });
            db.Products.AddRange(new List<Product>
            {
                new Product { Name = "Смартфон", Description = "Смартфон с 64ГБ памяти", Price = 299.99 },
                new Product { Name = "Ноутбук", Description = "Ноутбук с 8ГБ ОЗУ и 256ГБ SSD", Price = 799.99 },
                new Product { Name = "Наушники", Description = "Беспроводные наушники с шумоподавлением", Price = 99.99 },
                new Product { Name = "Клавиатура", Description = "Механическая клавиатура с подсветкой", Price = 49.99 }
            });
            db.SaveChanges();

            db.Orders.AddRange(new List<Order>
            {
                new Order { ClientId = 1, DeliveryAddress = "ул. Ленина, 10" },
                new Order { ClientId = 2, DeliveryAddress = "ул. Пушкина, 15" },
                new Order { ClientId = 3, DeliveryAddress = "ул. Советская, 45" },
                new Order { ClientId = 4, DeliveryAddress = "ул. Гагарина, 5" },
                new Order { ClientId = 1, DeliveryAddress = "ул. Киевская, 22" },
                new Order { ClientId = 2, DeliveryAddress = "ул. Набережная, 35" },
                new Order { ClientId = 3, DeliveryAddress = "ул. Октябрьская, 17" },
                new Order { ClientId = 4, DeliveryAddress = "ул. Водников, 6" },
                new Order { ClientId = 1, DeliveryAddress = "ул. Центральная, 13" },
                new Order { ClientId = 2, DeliveryAddress = "пр. Победы, 90" },
                new Order { ClientId = 3, DeliveryAddress = "ул. Парковая, 12" },
                new Order { ClientId = 4, DeliveryAddress = "ул. Дружбы, 8" },
                new Order { ClientId = 1, DeliveryAddress = "ул. Речная, 20" },
                new Order { ClientId = 2, DeliveryAddress = "ул. Солнечная, 3" },
                new Order { ClientId = 3, DeliveryAddress = "ул. Цветочная, 18" }
            });
            db.SaveChanges();

            db.OrderDetails.AddRange(new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, OrderId = 1, Quantity = 1 },
                    new OrderDetail { ProductId = 2, OrderId = 1, Quantity = 2 },
                    new OrderDetail { ProductId = 3, OrderId = 2, Quantity = 1 },
                    new OrderDetail { ProductId = 1, OrderId = 3, Quantity = 3 },
                    new OrderDetail { ProductId = 4, OrderId = 3, Quantity = 2 },
                    new OrderDetail { ProductId = 2, OrderId = 4, Quantity = 1 },
                    new OrderDetail { ProductId = 3, OrderId = 4, Quantity = 1 },
                    new OrderDetail { ProductId = 4, OrderId = 5, Quantity = 4 },
                    new OrderDetail { ProductId = 1, OrderId = 5, Quantity = 2 },
                    new OrderDetail { ProductId = 2, OrderId = 6, Quantity = 1 },
                    new OrderDetail { ProductId = 3, OrderId = 6, Quantity = 2 },
                    new OrderDetail { ProductId = 4, OrderId = 7, Quantity = 1 },
                    new OrderDetail { ProductId = 1, OrderId = 7, Quantity = 1 },
                    new OrderDetail { ProductId = 2, OrderId = 8, Quantity = 3 },
                    new OrderDetail { ProductId = 3, OrderId = 8, Quantity = 1 },
                    new OrderDetail { ProductId = 1, OrderId = 9, Quantity = 2 },
                    new OrderDetail { ProductId = 4, OrderId = 9, Quantity = 1 },
                    new OrderDetail { ProductId = 2, OrderId = 10, Quantity = 4 },
                    new OrderDetail { ProductId = 3, OrderId = 10, Quantity = 2 },
                    new OrderDetail { ProductId = 1, OrderId = 11, Quantity = 1 },
                    new OrderDetail { ProductId = 4, OrderId = 11, Quantity = 2 },
                    new OrderDetail { ProductId = 2, OrderId = 12, Quantity = 3 },
                    new OrderDetail { ProductId = 3, OrderId = 12, Quantity = 1 },
                    new OrderDetail { ProductId = 1, OrderId = 13, Quantity = 2 },
                    new OrderDetail { ProductId = 4, OrderId = 13, Quantity = 1 },
                    new OrderDetail { ProductId = 2, OrderId = 14, Quantity = 1 },
                    new OrderDetail { ProductId = 3, OrderId = 14, Quantity = 1 },
                    new OrderDetail { ProductId = 4, OrderId = 15, Quantity = 3 },
                    new OrderDetail { ProductId = 1, OrderId = 15, Quantity = 1 }
                });
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            // Вам необходимо написать один запрос LINQ to Entities, который извлекает список клиентов вместе со следующей информацией:
            //
            // Общее количество заказов для каждого клиента.
            // Общая сумма, потраченная каждым клиентов.
            // Самый дорогой товар, купленный каждым клиентом.
            
            // Запрос должен вернуть список объектов со следующими свойствами:
            // Имя клиента
            // Электронная почта
            // Адрес
            // Общее количество заказов
            // Общая потраченная сумма
            // Название самого дорогого приобретенного товара

            var clients = db.OrderDetails
                .GroupBy(e => e.Order.ClientId)
                .Select(e => new
                {
                    Client = e.Select(rd => rd.Order.Client.Name).FirstOrDefault(),
                    EmailClient = e.Select(rd => rd.Order.Client.Email).FirstOrDefault(),
                    Address = e.Select(rd => rd.Order.Client.Address).FirstOrDefault(),
                    TotalOrders = e.Count(),
                    TotalSum = e.Sum(rd => rd.Product.Price * rd.Quantity),
                    MostExpensiveProduct = e.Max(rd => rd.Product.Price)
                })
                .ToList();
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().Property(o => o.CreatedAt).HasDefaultValueSql("GETDATE()");
        
        base.OnModelCreating(modelBuilder);
    }
}