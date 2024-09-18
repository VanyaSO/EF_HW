using Microsoft.EntityFrameworkCore;

namespace HW_9;


// Создать таблицы: «Станция» и «Поезд». Используя метод FromSqlRaw и ExecuteSqlRaw, выполнить 8 запросов для получения данных:

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Train> Trains { get; set; }
}

public class Train
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Model { get; set; }
    public TimeSpan TravelTime { get; set; }
    public DateOnly ManufacturingDate { get; set; }
    public int StationId { get; set; }
    public Station Station { get; set; }
}


class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.ExecuteSqlRaw("INSERT INTO Stations (Name) VALUES ('Station1')");
            db.Database.ExecuteSqlRaw("INSERT INTO Trains (Number, Model, TravelTime, ManufacturingDate, StationId) VALUES ('777', 'Model7', '01:00:00', '2020-11-11', 1)");

            // Поезда у которых длительность маршрута более 5 часов.
            var trainsLongerThan5Hours = db.Trains.FromSqlRaw("SELECT * FROM Trains WHERE TravelTime > '05:00:00'").ToList();

            // Общую информация о станции и ее поездах.
            var stationsWithTrains = db.Stations.FromSqlRaw("SELECT * FROM Stations").Include(e => e.Trains).ToList();

            // Название станций у которой в наличии более 3-ех поездов.
            var stationsWithMoreThan3Trains = db.Stations.FromSqlRaw(
                "SELECT s.Name FROM Stations s " +
                "JOIN Trains t ON s.Id = t.StationId " +
                "GROUP BY s.Name " +
                "HAVING COUNT(*) > 3"
                )
                .ToList();

            // Все поезда, модель которых начинается на подстроку «Pell».
            var trainsWithModelStartingWithPell = db.Trains.FromSqlRaw("SELECT * FROM Trains WHERE Model LIKE 'Pell%'").ToList();

            // Все поезда, у которых возраст более 15 лет с текущей даты.
            var dateNow = DateTime.Now.Date;
            var trainsOlderThan15Years = db.Trains.FromSqlRaw("SELECT * FROM Trains WHERE ManufacturingDate <= DATEADD(year, -15, {0})", dateNow).ToList();

            // Получить станции, у которых в наличии хотя бы один поезд с длительность маршрутка менее 4 часов.
            var stationsWithShortTravelTimeTrains = db.Stations.FromSqlRaw(
                    "SELECT DISTINCT s.* FROM Stations s " +
                    "JOIN Trains t ON s.Id = t.StationId " +
                    "WHERE t.TravelTime <= '04:00:00'"
                    )
                .ToList();

            // Вывести все станции без поездов (на которых не будет поездов при выполнении LEFT JOIN).
            var stationsWithoutTrains = db.Stations
                .FromSqlRaw(
                    "SELECT s.Id, s.Name FROM Stations s " +
                    "LEFT JOIN Trains t ON s.Id = t.StationId " +
                    "WHERE t.StationId IS NULL"
                )
                .ToList();
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Station> Stations = null!;
    public DbSet<Train> Trains = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
        optionsBuilder.UseSqlServer("Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }
}