using System.Runtime.InteropServices.Marshalling;
using hw_1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DatabaseService
{
    private string _dbTrainsConnectionStringKey = "DefaultConnection"; 
    private string _dbMenuDishesConnectionStringKey = "DatabaseMenuDishes"; 
    private DbContextOptions<TContext> GetOptions<TContext>(string connectionStringKey) where TContext : DbContext
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("config.json");

        var config = builder.Build();
        string connectionString = config.GetConnectionString(connectionStringKey);
        
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        return optionsBuilder.UseSqlServer(connectionString).Options;
    }
    public async Task EnsurePopulateTrains()
    {
        using (ApplicationContext db = new ApplicationContext(GetOptions<ApplicationContext>(_dbTrainsConnectionStringKey)))
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Trains.AddRange(Train.GetMockData());
            await db.SaveChangesAsync();
        }
    }
    public async Task AddTrain(Train train) 
    {
        using (ApplicationContext db = new ApplicationContext(GetOptions<ApplicationContext>(_dbTrainsConnectionStringKey)))
        {
            db.Trains.Add(train);
            await db.SaveChangesAsync();
        }
    }
    public async Task<Train> GetTrain(int id)
    {
        using (ApplicationContext db = new ApplicationContext(GetOptions<ApplicationContext>(_dbTrainsConnectionStringKey)))
        {
            return await db.Trains.FirstOrDefaultAsync(item => item.Id == id);
        }
    }
    public async Task UpdateTrain(Train train)
    {
        using (ApplicationContext db = new ApplicationContext(GetOptions<ApplicationContext>(_dbTrainsConnectionStringKey)))
        {
            db.Trains.Update(train);
            await db.SaveChangesAsync();
        }
    }
    public async Task DeleteTrain(Train train)
    {
        using (ApplicationContext db = new ApplicationContext(GetOptions<ApplicationContext>(_dbTrainsConnectionStringKey)))
        {
            db.Trains.Remove(train);
            await db.SaveChangesAsync();
        }
    }
    
    // доп задача
    public async Task EnsurePopulateDishes()
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            await db.SaveChangesAsync();
        }
    }
    public bool IsMenuDishesDatabaseAvailable()
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            return db.Database.CanConnect();
        }
    }
    public async Task AddDish(Dish dish)
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            db.MenuDishes.Add(dish);
            await db.SaveChangesAsync();
        }
    }
    public async Task AddRangeDish(params Dish[] dishes)
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            db.MenuDishes.AddRange(dishes);
            await db.SaveChangesAsync();
        }
    }
    public async Task<List<Dish>> GetDishes()
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            return await db.MenuDishes.ToListAsync();
        }
    }

    public async Task<List<Dish>> GetDishByName(string name)
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            return await db.MenuDishes
                .Where(e => e.Name.ToUpper().Contains(name.ToUpper()))
                .ToListAsync();
        }
    }
    public async Task<Dish> GetDishById(int id)
    {
        using (MenuDishesContext db = new MenuDishesContext(GetOptions<MenuDishesContext>(_dbMenuDishesConnectionStringKey)))
        {
            return await db.MenuDishes.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}