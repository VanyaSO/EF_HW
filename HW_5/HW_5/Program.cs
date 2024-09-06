using Microsoft.EntityFrameworkCore;

namespace HW_5;
// Создайте систему для управления «Событиями» и «Гостями». 
// Создайте таблицы для событий, гостей и связывающую таблицу, которая представляет собой отношение «многие ко многим» между событиями и гостями.
// Дополнительно в этой связи создать дополнительную колонку для хранения «Роли гостя» на конкретном событии.
// Выполните следующие запросы к созданным таблицам (каждый запрос оформите в отдельный метод):

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            List<Guest> guests = new List<Guest> { new Guest { Name = "Ivan", Age = 20 }, new Guest { Name = "Gleb", Age = 25 }, new Guest { Name = "Alex", Age = 18 } };
            List<Event> events = new List<Event> { new Event { Name = "HP Olga" }, new Event { Name = "NY" } };
            db.Guests.AddRange(guests);
            db.Events.AddRange(events);
            db.SaveChanges();

            db.AddRange(new List<GuestInEvent>
            {
                new GuestInEvent{GuestId = guests[0].Id, EventId = events[0].Id, Role = Role.Guest},
                new GuestInEvent{GuestId = guests[1].Id, EventId = events[0].Id, Role = Role.Speaker},
                new GuestInEvent{GuestId = guests[1].Id, EventId = events[1].Id, Role = Role.Worker},
            });
            
            db.SaveChanges();
        }

        DbService service = new DbService();
        
        // 1) Добавление гостя на событие.
        Guest alex = service.GetGuest(3);
        Event ny = service.GetEvent(2);
        service.AddGuestInEvent(alex, ny, Role.Speaker);
        
        // 2) Получение списка гостей на конкретном событии.
        var listGuests = service.GetGuestsInEvent(2);
        
        // 3) Изменение роли гостя на событии.
        service.UpdateGuestRoleInEvent(1, 1, Role.Organizer); // 1-ivan / 1-hpOlga
        
        // 4) Получение всех событий для конкретного гостя.
        var listEventsForAlex = service.GetEventsForGuest(3);
        
        // 5) Удаление гостя с события.
        service.RemoveGuestInEvent(3, 2); // 3-alex / 2-ny
        
        // 6) Получение всех событий, на которых гость выступал в роли спикера.
        var listWhereGuestWasSpeaker = service.GetEventsByGuestRole(2, Role.Speaker);
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Guest> Guests { get; set; } = null!;
    public DbSet<GuestInEvent> GuestInEvents { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Guest>().HasMany(g => g.Events).WithMany(e => e.Guests).UsingEntity<GuestInEvent>(
            e => e.HasOne(e => e.Event).WithMany(s => s.GuestInEvents).HasForeignKey(e => e.EventId),
            e => e.HasOne(e => e.Guest).WithMany(g => g.GuestInEvents).HasForeignKey(e => e.GuestId),
            e =>
            {
                e.Property(e => e.Role);
                e.HasKey(e => e.Id);
                e.ToTable("GuestInEvents");
            }
        );
    }
}

// Сущности
public class Guest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Event> Events { get; set; }
    public List<GuestInEvent> GuestInEvents { get; set; }
}

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Guest> Guests { get; set; }
    public List<GuestInEvent> GuestInEvents { get; set; }
}

public enum Role
{
    Organizer,
    Guest,
    Worker,
    Speaker
}

public class GuestInEvent
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public int EventId { get; set; }
    public Role Role { get; set; }

    public Guest Guest { get; set; }
    public Event Event { get; set; }
}