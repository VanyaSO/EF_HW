using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace hw_4_dop_3;

// Написать программу, содержащую многоуровневую структуру данных. Описать классы: «Страна», «Аэропорт», «Самолет», «Характеристики самолета». Реализовать возможность получение полных данных, а самолете (сам самолет, его характеристики, аэропорт в котором он находится, и страна в которой находится аэропорт). Задачу можно реализовать, используя методы Include / ThenInclude или Lazy Loading. В основной части программы, реализовать возможности: 
// Добавление страны.
// Добавление аэропорта.
// Добавление самолета и его характеристик.
// Получение полных данных через самолет.
// Получение полных данных через страну и аэропорт.


class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Country Ukraine = new Country { Name = "Ukraine" };
            Country USA = new Country { Name = "USA" };

            Airplane airp1 = new Airplane { Name = "Plane1", AirplaneSettings = new AirplaneCharacteristics{NumberSeats = 32}};
            Airplane airp2 = new Airplane { Name = "Plane2", AirplaneSettings = new AirplaneCharacteristics{NumberSeats = 2} };
            Airplane airp3 = new Airplane { Name = "Plane3", AirplaneSettings = new AirplaneCharacteristics{NumberSeats = 7}};
            
            Airport Ap1 = new Airport { Name = "Airport 1", Airplanes = new() {airp1, airp2}, Country = Ukraine};
            Airport Ap2 = new Airport { Name = "Airport 2", Airplanes = new() {airp1, airp3}, Country = USA};
            
            db.Airports.AddRange(Ap1, Ap2);
            db.SaveChanges();
            
            // Реализовать возможность получение полных данных, а самолете
            // (сам самолет, его характеристики, аэропорт в котором он находится, и страна в которой находится аэропорт
            var aboutPlane = db.Airplanes
                .Include(a => a.AirplaneSettings)
                .Include(a => a.Airports)
                .ThenInclude(air => air.Country)
                .ToList();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            DbService dbService = new DbService();
            dbService.AddAirplane(new Airplane{Name = "AddedName"});
            dbService.AddCountry(new Country(){Name = "NewCountry"});

            Country? country = dbService.GetCountry(2);
            Airplane? airplane = dbService.GetAirplane(1);
            Airport? airport = dbService.GetAirport(2);
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Airport> Airports { get; set; } = null!;
    public DbSet<Airplane> Airplanes { get; set; } = null!;
    public DbSet<AirplaneCharacteristics> AirplaneCharacteristics { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=CW;User=sa;Password=admin@Admin87457; TrustServerCertificate = True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airplane>(e =>
        {
            e.HasOne(a => a.AirplaneSettings)
                .WithOne(apls => apls.Airplane);
            
            e.HasMany(a => a.Airports)
                .WithMany(air => air.Airplanes);
        });

        modelBuilder.Entity<Airport>()
            .HasOne(a => a.Country)
            .WithMany(c => c.Airports);
    }
}

public class Country
{
  public int Id { get; set; }
  public string Name { get; set; }
  public List<Airport> Airports { get; set; } = new ();
}

public class Airport
{
  public int Id { get; set; }
  public string Name { get; set; }
  public List<Airplane> Airplanes { get; set; } = new ();
  public int CountryId { get; set; }
  public Country Country { get; set; }
}

public class Airplane
{
  public int Id { get; set; }
  public string Name { get; set; }
  public AirplaneCharacteristics AirplaneSettings { get; set; }
  public List<Airport> Airports { get; set; } = new();
}

public class AirplaneCharacteristics
{
  public int Id { get; set; }
  public int NumberSeats { get; set; }
  public int AirplaneId { get; set; }
  public Airplane Airplane { get; set; }
}

