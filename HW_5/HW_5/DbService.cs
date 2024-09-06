using HW_5;
using Microsoft.EntityFrameworkCore;

public class DbService
{
    // 1) Добавление гостя на событие.
    public void AddGuestInEvent(Guest guest, Event _event, Role role)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            if (guest == null && _event == null) return;
            db.GuestInEvents.Add(new GuestInEvent{GuestId = guest.Id, EventId = _event.Id, Role = role});
            db.SaveChanges();
        }
    }
    
    // 2) Получение списка гостей на конкретном событии.
    public List<Guest> GetGuestsInEvent(int eventId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Guests.Include(g=> g.Events).Where(g => g.Events.Any(e => e.Id == eventId)).ToList();
        }
    }
    
    // 3) Изменение роли гостя на событии.
    public void UpdateGuestRoleInEvent(int guestId, int eventId, Role newRole)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            GuestInEvent guestInEvent = db.GuestInEvents.FirstOrDefault(e => e.GuestId == guestId && e.EventId == eventId);
            guestInEvent.Role = newRole;
            db.GuestInEvents.Update(guestInEvent);
            db.SaveChanges();
        }
    }
    
    // 4) Получение всех событий для конкретного гостя.
    public List<Event> GetEventsForGuest(int guestId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Events.Include(e=>e.Guests).Where(e => e.Guests.Any(g => g.Id == guestId)).ToList();
        }
    }
    
    // 5) Удаление гостя с события.
    public void RemoveGuestInEvent(int guestId, int eventId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            GuestInEvent guestInEvent = db.GuestInEvents.FirstOrDefault(e => e.GuestId == guestId && e.EventId == eventId);
            db.GuestInEvents.Remove(guestInEvent);
            db.SaveChanges();
        }
    }
    
    // 6) Получение всех событий, на которых гость выступал в роли спикера.
    public List<Event> GetEventsByGuestRole(int guestId, Role role)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Events.Include(e => e.Guests).Where(e => e.GuestInEvents.Any(gie => gie.GuestId == guestId && gie.Role == role)).ToList();
        }
    }
    
    public Event? GetEvent(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Events.Include(e => e.Guests).FirstOrDefault(e => e.Id == id);
        }
    }
    
    public Guest? GetGuest(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Guests.Include(e => e.Events).FirstOrDefault(e => e.Id == id);
        }
    }
}