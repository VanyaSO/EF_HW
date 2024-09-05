using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;

namespace hw_4_dop_1;

// Создайте базу данных, представляющую информацию о «Компаниях», их «Сотрудниках» и «Проектах».
// Необходимо создать запрос с использованием Entity Framework Core
// для получения списка проектов, в которых участвуют сотрудники из определенной компании.
// Создать два типа связи: компания – сотрудники (один ко многим), сотрудники – проекты (многие ко многим).

class Program
{
    static void Main(string[] args)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            List<Company> companies = new List<Company>()
            {
                new Company
                {
                    Name = "Armar",
                    Users = new List<User>
                    {
                        new User
                        {
                            FirstName = "Ivan", LastName = "Ushachov",
                            Projects = new List<Project>
                            {
                                new Project { Name = "Lb" }, new Project { Name = "Nesia" }
                            }
                        },
                        new User
                        {
                            FirstName = "User1", LastName = "UserLastName1",
                            Projects = new List<Project> { new Project { Name = "Yolo" } }
                        }
                    },
                },
                new Company
                {
                    Name = "More",
                    Users = new List<User>
                    {
                        new User { FirstName = "Alex", LastName = "Sergee", Projects = new List<Project>{new Project{Name = "Lumo"}}},
                    },
                }
            };
            db.Companies.AddRange(companies);
            db.SaveChanges();
            
            User? ivan = db.Users.FirstOrDefault(u => u.FirstName == "Ivan");
            Project? lumo = db.Projects.FirstOrDefault(p => p.Name == "Yolo");
            ivan.Projects.Add(lumo);
            db.Projects.Add(new Project{Name="Bybit"});
            
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            // Необходимо создать запрос с использованием Entity Framework Core  для получения списка проектов,
            // в которых участвуют сотрудники из определенной компании.
            var list = db.Projects.Where(p => p.Users.Any(u => u.Company.Name == "Armar")).ToList();
        }
    }
}

public class ApplicationContext : DbContext
{
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}