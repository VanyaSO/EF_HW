using hw_1;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Train> Trains { get; set; }
    
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }
}