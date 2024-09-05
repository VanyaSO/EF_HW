using Microsoft.EntityFrameworkCore;
using hw_4_dop_3;

public class DbService
{
    // В основной части программы, реализовать возможности: 
    // Добавление страны.
    public void AddCountry(Country country)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Countries.Add(country);
            db.SaveChanges();
        }
    }

    // Добавление аэропорта.
    public void AddAirport(Airport airport)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Airports.Add(airport);
            db.SaveChanges();
        }
    }

    // Добавление самолета и его характеристик.
    public void AddAirplane(Airplane airplane)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Airplanes.Add(airplane);
            db.SaveChanges();
        }
    }

    // Получение полных данных через самолет.
    public Airplane? GetAirplane(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Airplanes
                .Include(a => a.AirplaneSettings)
                .Include(a => a.Airports)
                .ThenInclude(air => air.Country)
                .FirstOrDefault(p => p.Id == id);
        }
    }

    // Получение полных данных через страну и аэропорт.
    public Country? GetCountry(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Countries
                .Include(c => c.Airports)
                .ThenInclude(a => a.Airplanes)
                .ThenInclude(p => p.AirplaneSettings)
                .FirstOrDefault(c => c.Id == id);
        }
    }

    public Airport? GetAirport(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Airports
                .Include(a => a.Airplanes)
                .Include(a => a.Country)
                .FirstOrDefault(a => a.Id == id);
        }
    }
}