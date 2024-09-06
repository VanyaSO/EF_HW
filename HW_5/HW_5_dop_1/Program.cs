using Microsoft.EntityFrameworkCore;

namespace HW_5_dop_1;
// Создайте приложение, которое содержит таблицу «Меню».
// Данная таблица содержит пункты меню, при этом один пункт, может содержать сколько угодно
// подпунктов или ни одного и при этом сам может иметь родительский пункт меню.
// Написать запрос для получения подобной иерархии:
// -File
// --Open
// --Save
// --Save As
// ---To hard-drive..
// ---To online-drive..
// -Edit
// -View

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Menus.Add(new Menu
            {
                Name = "Меню",
                SubMenus = new List<Menu>
                {
                    new Menu { Name = "Главная" },
                    new Menu
                    {
                        Name = "Каталог",
                        SubMenus = new List<Menu>
                        {
                            new Menu { Name = "Сумки" },
                            new Menu
                            {
                                Name = "Портфели",
                                SubMenus = new List<Menu>
                                {
                                    new Menu { Name = "Школьные" }
                                }
                            },
                        }
                    },
                    new Menu { Name = "Контакты" }
                }
            });
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            var menu = db.Menus.ToList();
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Menu> Menus { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }
}

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Url { get; set; }
    public int? ParentId { get; set; }
    public List<Menu> SubMenus { get; set; }
}