using hw_1;
using Microsoft.EntityFrameworkCore;

public class MenuDishesContext : DbContext
{
    public DbSet<Dish> MenuDishes { get; set; }
    
    public MenuDishesContext(DbContextOptions options) : base(options)
    {
    }
}