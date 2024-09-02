using HW_3;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectingString = "Server=localhost;Database=MyUsers;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectingString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasData(
                new User { Id = 1, Name = "Ivan", Email = "ushachov@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123") },
                new User { Id = 2, Name = "Gleb", Email = "gleb@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234") },
                new User { Id = 3, Name = "Max", Email = "max@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("12345") }
            );
            e.HasIndex(e => e.Email).IsUnique();
            e.Property(e => e.isBlocked).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.HasData(
                new Book { Id = 1, Name = "1984", Author = "George Orwell" },
                new Book { Id = 2, Name = "To Kill a Mockingbird", Author = "Harper Lee" },
                new Book { Id = 3, Name = "The Great Gatsby", Author = "F. Scott Fitzgerald" },
                new Book { Id = 4, Name = "One Hundred Years of Solitude", Author = "Gabriel Garcia Marquez" },
                new Book { Id = 5, Name = "Pride and Prejudice", Author = "Jane Austen" },
                new Book { Id = 6, Name = "The Catcher in the Rye", Author = "J.D. Salinger" },
                new Book { Id = 7, Name = "Moby-Dick", Author = "Herman Melville" },
                new Book { Id = 8, Name = "War and Peace", Author = "Leo Tolstoy" },
                new Book { Id = 9, Name = "The Odyssey", Author = "Homer" },
                new Book { Id = 10, Name = "Ulysses", Author = "James Joyce" },
                new Book { Id = 11, Name = "The Divine Comedy", Author = "Dante Alighieri" },
                new Book { Id = 12, Name = "Crime and Punishment", Author = "Fyodor Dostoevsky" },
                new Book { Id = 13, Name = "The Brothers Karamazov", Author = "Fyodor Dostoevsky" },
                new Book { Id = 14, Name = "Brave New World", Author = "Aldous Huxley" }
            );
        });
    }
}