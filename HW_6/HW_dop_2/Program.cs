using Microsoft.EntityFrameworkCore;

namespace HW_dop_2;

// Создать приложение по управлению «Пользователями» и их «Компаниями». Реализовать следующие операции: 
// 1) Добавление пользователей и компаний. 
// 2) Редактирование, удаление, поиск - пользователей и компаний.
// 3) Вывод полной информации - пользователей и компаний.

interface IPrint
{
    public void Print();
}

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        DbService service = new DbService();

        // Добавление пользователей и компаний
        service.Add(new Company { Name = "More", Users = new List<User>{new User { Name = "Alex"}}}, new Company { Name = "Luxoft", Users = new List<User>{new User { Name = "Ivan"}} });
        service.Add(new User{Name = "Dima", CompanyId = 1});
        
        // Редактирование, удаление, поиск
        User? dima = service.FindUser(3);
        dima.CompanyId = 2;
        service.Update(dima);

        Company? more = service.FindCompany(1);
        service.Remove(more);
        
        // Вывод полной информации - пользователей и компаний.
        Printer(dima);
        Company? luxoft = service.FindCompany(2);
        Printer(luxoft);
    }

    public static void Printer(IPrint obj) => obj.Print();
};

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class User : IPrint
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? CompanyId { get; set; }

    public Company? Company { get; set; }

    public void Print()
    {
        Console.Write($"Id: {Id}, Name: {Name} ");
        if (Company != null)
        {
            Console.Write($"CompanyName: {Company.Name}");
        }
        Console.WriteLine();
    }
}

public class Company : IPrint
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; }

    public void Print()
    {
        Console.WriteLine($"Id: {Id}, Name: {Name}");
        Console.WriteLine("Users");
        foreach (var user in Users)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
        Console.WriteLine();
    }
}