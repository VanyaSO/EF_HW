using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;

namespace HW_dop_1;

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            List<City> cities = new List<City>
            {
                new City {Name = "Киев" },
                new City {Name = "Днепр" },
            };
            db.Cities.AddRange(cities);
            db.SaveChanges();
        
            List<Product> products = new List<Product>
            {
                new Product { Name = "Роза 60см", Price = 25.00, Type = ProductType.Flowers},
                new Product { Name = "Роза 30см", Price = 15.00, Type = ProductType.Flowers },
                new Product { Name = "Тюльпан", Price = 10.00, Type = ProductType.Flowers },
                new Product { Name = "Упаковачная бумага", Price = 5.00, Type = ProductType.Decoration },
                new Product { Name = "Подарочная коробка", Price = 7.50, Type = ProductType.Box },
            };
            db.Products.AddRange(products);
            db.SaveChanges();
        
            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier { Name = "Цветочный магнат"},
                new Supplier { Name = "Все для цветов"},
            };
            db.Suppliers.AddRange(suppliers);
            db.SaveChanges();
        
            List<Store> stores = new List<Store>
            {
                new Store
                {
                    Name = "Цветочный веник", 
                    CityId = cities[0].Id, 
                    Address = "Центральная 24",
                    Suppliers = new List<Supplier> { suppliers[0] }, 
                },
                new Store
                {
                    Name = "Феерия",
                    CityId = cities[1].Id, 
                    Address = "Ольгиевская 4",
                    Suppliers = new List<Supplier> { suppliers[1] }, 
                },
                new Store
                {
                    Name = "Тюльпанчик",
                    CityId = cities[0].Id, 
                    Address = "Торговая 1",
                    Suppliers = new List<Supplier> { suppliers[0] }, 
                }
            };
            db.Stores.AddRange(stores);
            db.SaveChanges();
        
            List<StoreProduct> storeProducts = new List<StoreProduct>
            {
                new StoreProduct
                {
                    StoreId = stores[0].Id,
                    ProductId = products[0].Id,
                    QuantityInStock = 50
                },
                new StoreProduct
                {
                    StoreId = stores[0].Id,
                    ProductId = products[1].Id,
                    QuantityInStock = 30
                },
                new StoreProduct
                {
                    StoreId = stores[0].Id,
                    ProductId = products[4].Id,
                    QuantityInStock = 20
                },
                new StoreProduct
                {
                    StoreId = stores[1].Id,
                    ProductId = products[1].Id,
                    QuantityInStock = 40
                },
                new StoreProduct
                {
                    StoreId = stores[1].Id,
                    ProductId = products[3].Id,
                    QuantityInStock = 25
                },
                new StoreProduct
                {
                    StoreId = stores[1].Id,
                    ProductId = products[4].Id,
                    QuantityInStock = 15
                },
                new StoreProduct
                {
                    StoreId = stores[2].Id,
                    ProductId = products[2].Id,
                    QuantityInStock = 60
                },
                new StoreProduct
                {
                    StoreId = stores[2].Id,
                    ProductId = products[3].Id,
                    QuantityInStock = 35
                }
            };
            db.StoreProducts.AddRange(storeProducts);
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            // 1) Поиск товара по определенному магазину (Используя оператор LIKE in SQL).
            string findProduct = "роз";
            var task1 = db.StoreProducts
                .Include(e => e.Product)
                .Where(e => e.StoreId == 1 && EF.Functions.Like(e.Product.Name.ToUpper(), $"%{findProduct.ToUpper()}%"))
                .Select(e => e.Product)
                .ToList();
            
            // 2) Поиск товара по всем магазинам. (Используя оператор LIKE in SQL).
            var task2 = db.StoreProducts
                .Include(e => e.Product)
                .Where(e => EF.Functions.Like(e.Product.Name.ToUpper(), $"%{findProduct.ToUpper()}%"))
                .Select(e => e.Store)
                .Distinct()
                .ToList();
            
            // 3) Получение случайного товара из определенного магазина.
            var task3 = db.Stores
                .Where(e => e.Id == 3)
                .SelectMany(e => e.Products)
                .OrderBy(p => EF.Functions.Random())
                .FirstOrDefault();

            // 4) Сортировка цветов: по убыванию и возрастанию их стоимости.
            var task4 = db.Products
                .Where(p => p.Type == ProductType.Flowers)
                .OrderBy(p => p.Price)
                .ToList();
            
            var task4_2 = db.Products
                .Where(p => p.Type == ProductType.Flowers)
                .OrderByDescending(p => p.Price)
                .ToList();

            // 5) На основе анонимного типа, сформировать отчет, содержащий:
            // общее количество товара в наличии и общую сумму товара для каждого из магазинов в определенном городе (выбор города по ID).
            var task5 = db.Stores
                .Where(s => s.CityId == 1)
                .Select(s => new
                {
                    Name = s.Name,
                    TotalQuantity = s.StoreProducts.Sum(e => e.QuantityInStock),
                    TotalSumProducts = s.StoreProducts.Sum(e => e.QuantityInStock * e.Product.Price)
                })
                .ToList();
            
            // 6) Вывести для владельца общую информацию о его бизнесе  виде:
            // -- Город
            // -- Магазин 1
            // -- Товары: ...
            // -- Магазин 2
            // -- Товары: …
            // и т.д..
            var task6 = db.Cities
                .Select(c => new
                {
                    City = c.Name,
                    Stores = c.Stores.Select(s => new
                    {
                        Name = s.Name,
                        Address = s.Address,
                        Supplier = s.Suppliers.Select(su => new
                        {
                            Name = su.Name
                        }).ToList(),
                        Products = s.Products.Select(p => new
                        {
                            Name = p.Name,
                            Price = p.Price,
                            Type = p.Type,
                            Quantity = p.StoreProducts.FirstOrDefault(e => e.ProductId == p.Id && e.StoreId == s.Id).QuantityInStock
                        }).ToList()
                    }).ToList()
                }).ToList();
            
            
            // 7) Вывести товары из определенного магазина, сумма которых превосходит N гривен.
            var task7 = db.StoreProducts
                .Include(e => e.Product)
                .Where(e => e.StoreId == 1 && e.Product.Price > 10)
                .Select(e => e.Product)
                .ToList();
            
            // 8) Вывести названия магазинов, у которых количество превосходит N единиц.
            var task8 = db.StoreProducts
                .Include(e => e.Store)
                .Where(e => e.QuantityInStock > 15)
                .Distinct()
                .Select(e => e.Product.Name)
                .ToList();
            
            // 9) Реализовать получение магазинов определенного города через Свойство Shops, в модели Country.
            // Так как нету Country сделал через Cities
            var task9 = db.Cities
                .Where(c => c.Id == 1)
                .SelectMany(c => c.Stores)
                .ToList();

            // 10) Получить среднюю стоимость по каждому магазину из разных городов, по типу:
            // Киев:
            // Магазин "Цветочный веник": Средняя стоимость : 155
            // Магазин "Феерия": Средняя стоимость : 172
            // Днепр:
            // Магазин "Тюльпанчик": Средняя стоимость : 188
            // Магазин "У бабушки Гали": Средняя стоимость : 177
            var task10 = db.Cities
                .Select(e => new
                {
                    City = e.Name,
                    Stores = e.Stores.Select(s => new
                    {
                        Name = s.Name,
                        AveragePrice = s.Products.Average(p => p.Price)
                    }).ToList()
                }).ToList();
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<StoreProduct> StoreProducts { get; set; } = null!;
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>().HasMany(s => s.Products).WithMany(p => p.Stores).UsingEntity<StoreProduct>(
            e => e.HasOne(e => e.Product).WithMany(s => s.StoreProducts).HasForeignKey(e => e.ProductId),
            e => e.HasOne(e => e.Store).WithMany(s => s.StoreProducts).HasForeignKey(e => e.StoreId),
            e =>
            {
                e.ToTable("StoresProducts");
            }
        );
        
        modelBuilder.Entity<StoreProduct>()
            .Property(s => s.QuantityInStock)
            .HasDefaultValueSql("ABS(CHECKSUM(NEWID()) % 30) + 0");
    }
}