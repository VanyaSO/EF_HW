using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace HW_4;

// Реализовать 2 класса: «Пользователь» и «Настройки пользователя».
// Организовать между таблицами связь один к одному.
// Добавить несколько пользователей и их настройки.
// Достать пользователя с Id = 2 и его настройки.
// Удалить пользователя с Id 3 (автоматически должен удалится профайл пользователя).

class Program
{
    static void Main(string[] args)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users = new List<User>
            {
                new User { Email = "user1@example.com", Password = "password1" },
                new User { Email = "user2@example.com", Password = "password2" },
                new User { Email = "user3@example.com", Password = "password3" },
                new User { Email = "user4@example.com", Password = "password4" },
                new User { Email = "user5@example.com", Password = "password5" }
            };
            db.Users.AddRange(users);
            db.SaveChanges();

            var userSettings = new List<UserSettings>
            {
                new UserSettings
                {
                    FirstName = "John", LastName = "Doe", PhoneNumber = "123-456-7890", Passport = "AB1234567",
                    Age = 30, UserId = 1
                },
                new UserSettings
                {
                    FirstName = "Jane", LastName = "Smith", PhoneNumber = "234-567-8901",
                    Passport = "CD2345678", Age = 28, UserId = 2
                },
                new UserSettings
                {
                    FirstName = "Alice", LastName = "Johnson", PhoneNumber = "345-678-9012",
                    Passport = "EF3456789", Age = 35, UserId = 3
                },
                new UserSettings
                {
                    FirstName = "Bob", LastName = "Williams", PhoneNumber = "456-789-0123",
                    Passport = "GH4567890", Age = 40, UserId = 4
                },
                new UserSettings
                {
                    FirstName = "Eve", LastName = "Brown", PhoneNumber = "567-890-1234", Passport = "IJ5678901",
                    Age = 25, UserId = 5
                }
            };
            db.UserSettings.AddRange(userSettings);
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            // Удалить пользователя с Id 3
            User? removeUser = db.Users.FirstOrDefault(e => e.Id == 3);
            if (removeUser != null)
            {
                db.Users.Remove(removeUser);
                db.SaveChanges();
            }
        }
    }
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserSettings> UserSettings { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserSettings)
            .WithOne(s => s.User)
            .OnDelete(DeleteBehavior.Cascade);
    }
}