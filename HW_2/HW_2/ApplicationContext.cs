using Microsoft.EntityFrameworkCore;

namespace HW_2;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderHistory> OrderHistories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectingString = "Server=localhost;Database=Shop;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectingString);
    }
}