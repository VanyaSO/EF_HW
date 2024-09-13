using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;

namespace HW_7_dop_1;

// Создайте базу данных «Список рассылки», для списка рассылки об акционных товарах. Нужно хранить такую информацию:
// ■ ФИО покупателя; 
// ■ Дата рождения покупателя; 
// ■ Пол покупателя; 
// ■ Email покупателя; 
// ■ Страна покупателя; 
// ■ Город покупателя; 
public class Client
{
    public int Id { get; set; }
    public string Fio { get; set; }
    public DateTime Birthday { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public int? CountryId { get; set; }
    public int? CityId { get; set; }

    public Country? Country { get; set; }
    public City? City { get; set; }
    public List<Section> Sections { get; set; }
}

public enum Gender
{
    Male,
    Female
}

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<City> Cities { get; set; }
    public List<Client> Clients { get; set; }
    public List<SaleProduct> SaleProducts { get; set; }
}

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? CountryId { get; set; }
    public Country? Country { get; set; }
    public List<Client> Clients { get; set; }
}

// ■ Список разделов, в которых заинтересован покупатель. Например: мобильные телефоны, ноутбуки, кухонная техника и т.д.;
public class Section
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SaleProduct> SaleProducts { get; set; }
    public List<Client> Clients { get; set; }
}

// ■ Акционные товары по каждому разделу. Акции привязаны к стране. У каждой акции есть время действия (дата старта, дата конце).
public class SaleProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? CountryId { get; set; }
    public DateTime StartSale { get; set; }
    public DateTime EndSale { get; set; }

    public Country? Country { get; set; }
    public List<Section> Sections { get; set; }
}

// Создайте приложение, которое позволит пользователю подключиться и отключиться от базы данных «Список рассылки». Используя Entity Framework, добавьте к приложению следующую функциональность (каждое действие в отдельном методе):
class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Countries.AddRange(new List<Country>
            {
                new Country { Name = "Германия" },
                new Country { Name = "Япония" }
            });
            db.SaveChanges();

            db.Cities.AddRange(new List<City>
            {
                new City { Name = "Берлин", CountryId = db.Countries.FirstOrDefault(e => e.Id == 1).Id },
                new City { Name = "Токио", CountryId = db.Countries.FirstOrDefault(e => e.Id == 2).Id }
            });
            db.SaveChanges();

            db.Sections.AddRange(new List<Section>
            {
                new Section { Name = "Мобильные телефоны" },
                new Section { Name = "Ноутбуки" },
                new Section { Name = "Кухонная техника" },
                new Section { Name = "Телевизоры" },
                new Section { Name = "Фотоаппараты" }
            });
            db.SaveChanges();
            
            db.Clients.AddRange(new List<Client>
            {
                new Client
                {
                    Fio = "Петрова Мария Сергеевна", Birthday = new DateTime(1985, 5, 12), Gender = Gender.Female,
                    Email = "petrova@mail.de", CountryId = 1, CityId = 1, Sections = new List<Section> { db.Sections.FirstOrDefault(e => e.Id == 1)}
                },
                new Client
                {
                    Fio = "Иванов Сергей Петрович", Birthday = new DateTime(1985, 5, 12), Gender = Gender.Male,
                    Email = "petrova@mail.de", CountryId = 1, CityId = 1
                },
                new Client
                {
                    Fio = "Сато Такеши", Birthday = new DateTime(1978, 10, 22), Gender = Gender.Male,
                    Email = "takeshi@mail.jp", CountryId = 2, CityId = 2, 
                }
            });
            db.SaveChanges();

            var sectionsIds = db.Sections.Select(e => e.Id).ToList();
            db.SaleProducts.AddRange(new List<SaleProduct>
            {
                new SaleProduct
                {
                    Name = "iPhone 14", CountryId = 1, Sections = new List<Section>(sectionsIds[0]), StartSale = new DateTime(2024, 1, 1),
                    EndSale = new DateTime(2024, 1, 15)
                },
                new SaleProduct
                {
                    Name = "Samsung Galaxy S23", CountryId = 1, Sections = new List<Section>(sectionsIds[0]), StartSale = new DateTime(2024, 2, 1),
                    EndSale = new DateTime(2024, 2, 15)
                },
                new SaleProduct
                {
                    Name = "MacBook Air M2", CountryId = 1, Sections = new List<Section>(sectionsIds[1]), StartSale = new DateTime(2024, 3, 1),
                    EndSale = new DateTime(2024, 3, 15)
                },
                new SaleProduct
                {
                    Name = "Dell XPS 13", CountryId = 1, Sections = new List<Section>(sectionsIds[1]), StartSale = new DateTime(2024, 4, 1),
                    EndSale = new DateTime(2024, 4, 15)
                },
                new SaleProduct
                {
                    Name = "Bosch Посудомоечная машина", CountryId = 1, Sections = new List<Section>(sectionsIds[2]),
                    StartSale = new DateTime(2024, 5, 1), EndSale = new DateTime(2024, 5, 15)
                },
                new SaleProduct
                {
                    Name = "Sony Bravia 65\"", CountryId = 2, Sections = new List<Section>(sectionsIds[4]), StartSale = new DateTime(2024, 6, 1),
                    EndSale = new DateTime(2024, 6, 15)
                },
                new SaleProduct
                {
                    Name = "Canon EOS R5", CountryId = 2, Sections = new List<Section>(sectionsIds[4]), StartSale = new DateTime(2024, 7, 1),
                    EndSale = new DateTime(2024, 7, 15)
                },
                new SaleProduct
                {
                    Name = "Nikon Z6 II", CountryId = 2, Sections = new List<Section>(sectionsIds[4]), StartSale = new DateTime(2024, 8, 1),
                    EndSale = new DateTime(2024, 8, 15)
                }
            });
            db.SaveChanges();
        }

        DbService sv = new DbService();
        
        // Отображение всех покупателей; 
        var clients = sv.GetClients();
        Printer.PrintClients(clients);
        
        // ■ Отображение email всех покупателей; 
        Printer.PrintEmailClients(clients);
        
        // ■ Отображение списка разделов; 
        var sections = sv.GetSections();
        Printer.PrintSections(sections);
        
        // ■ Отображение списка аукционных товаров; 
        var saleProducts = sv.GetSaleProducts();
        Printer.PrintSaleProducts(saleProducts);

        // ■ Отображение всех городов; 
        var cities = sv.GetCities();
        Printer.PrintCities(cities);
        
        // ■ Отображение всех стран.
        var countries = sv.GetCountries();
        Printer.PrintCountries(countries);
        
        // ■ Отображение всех покупателей из конкретного города;
        var clientsFromBerlin = sv.GetClientsByCity(1);
        Printer.PrintClients(clientsFromBerlin);
        
        // ■ Отображение всех покупателей из конкретной страны; 
        var clientsFromGermany = sv.GetClientsByCountry(1);
        Printer.PrintClients(clientsFromGermany);
        
        // ■ Отображение всех акций для конкретной страны.
        var saleProductsFromGermany = sv.GetSaleProductsByCountry(1);
        Printer.PrintSaleProducts(saleProductsFromGermany);
        
        // ■ Отображение списка городов конкретной страны; 
        var citiesFromGermany = sv.GetCitiesByCountry(1);
        Printer.PrintCities(citiesFromGermany);

        // ■ Отображение списка разделов конкретного покупателя; 
        var sectionsByClient = sv.GetSectionsByClient(1);
        Printer.PrintSections(sectionsByClient);

        // ■ Отображение списка аукционных товаров конкретного раздела.
        var saleProductFromSection = sv.GetSaleProductsBySection(1);
        Printer.PrintSaleProducts(saleProductFromSection);
        
        // ■ Обновление информации о покупателях;
        var updateClient = sv.GetClients().FirstOrDefault();
        updateClient.Sections.Add(sections[3]);
        sv.UpdateClient(updateClient);
        
        // ■ Обновление информации о странах;
        countries[0].Name = "NewName";
        sv.UpdateCountry(countries[0]);

        // ■ Обновление информации о городах; 
        cities[0].Name = "NewNameCity";
        sv.UpdateCity(cities[0]);


        // ■ Удаление информации о странах; 
        // sv.RemoveCountry(countries[0]);
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Section> Sections { get; set; } = null!;
    public DbSet<SaleProduct> SaleProducts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasOne(e => e.Country)
            .WithMany(e => e.Cities)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<SaleProduct>()
            .HasOne(e => e.Country)
            .WithMany(e => e.SaleProducts)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Client>()
            .HasOne(e => e.City)
            .WithMany(e => e.Clients)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Client>()
            .HasOne(c => c.Country)
            .WithMany(c => c.Clients)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }

}