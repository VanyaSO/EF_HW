using Microsoft.EntityFrameworkCore;

namespace HW_9_dop_1;
// Используя таблицы «Users» и «Companies», создать 3 хранимых процедуры:
// Получение связанных данных о Users и Companies.
// Используя входной параметр получить пользователей с именем наподобие “Tom”.
// Используя выходной параметр, получить средний возраст по всей таблицы пользователей.

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; }
}


class Program
{
    static void Main()
    {
        // using (ApplicationContext db = new ApplicationContext())
        // {
            // db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();
            // db.Users.Add(new User { Name = "Tomi", Company = new Company { Name = "More" } });
            // db.SaveChanges();
        // }

        using (ApplicationContext db = new ApplicationContext())
        {
            var users = db.Users
                .FromSqlRaw("GetAllUsersWithCompanies")
                .ToList();
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Users> Companies { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }
    
}