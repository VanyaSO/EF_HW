using Microsoft.EntityFrameworkCore;

namespace HW_7_dop_1;

public class DbService
{
    public List<Client> GetClients()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Clients
                .Include(e => e.Country)
                .Include(e => e.City)
                .Include(e => e.Sections)
                .ToList();
        }
    }

    public List<Client> GetClientsByCity(int cityId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Clients
                .Where(e => e.CityId == cityId)
                .ToList();
        }
    }
    
    public List<Client> GetClientsByCountry(int countryId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Clients
                .Where(e => e.CountryId == countryId)
                .ToList();
        }
    }
    
    public List<Section> GetSections()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Sections.ToList();
        }
    }
    
    public List<Section>? GetSectionsByClient(int clientId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Client? client = db.Clients.Include(e => e.Sections).FirstOrDefault(e => e.Id == clientId);
            return client?.Sections;
        }
    }
    
    public List<SaleProduct> GetSaleProducts()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.SaleProducts.ToList();
        }
    }
    
    public List<SaleProduct> GetSaleProductsByCountry(int countryId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.SaleProducts
                .Where(e => e.CountryId == countryId)
                .ToList();
        }
    }
    
    public List<SaleProduct> GetSaleProductsBySection(int sectionId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.SaleProducts
                .Where(e => e.Sections.Any(s => s.Id == sectionId))
                .ToList();
        }
    }
    
    public List<City> GetCities()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Cities.ToList();
        }
    }
    
    public List<City> GetCitiesByCountry(int countryId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Cities
                .Where(e => e.CountryId == countryId)
                .ToList();
        }
    }
    
    public List<Country> GetCountries()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Countries.ToList();
        }
    }
    
    // ■ Вставка информации о новых покупателях; 
    public void AddClient(Client client)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Clients.Add(client);
            db.SaveChanges();
        }
    }
    
    // ■ Вставка новых стран; 
    public void AddCountry(Country country)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Countries.Add(country);
            db.SaveChanges();
        }
    }
    
    // ■ Вставка новых городов; 
    public void AddCity(City city)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Cities.Add(city);
            db.SaveChanges();
        }
    }
    
    // ■ Вставка информации о новых разделах; 
    public void AddSection(Section section)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Sections.Add(section);
            db.SaveChanges();
        }
    }
    
    // ■ Вставка информации о новых аукционных товарах.
    public void AddSaleProduct(SaleProduct saleProduct)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.SaleProducts.Add(saleProduct);
            db.SaveChanges();
        }
    }
    
    // ■ Обновление информации о покупателях; 
    public void UpdateClient(Client client)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var currentClient = db.Clients
                .Include(e => e.City).ThenInclude(e => e.Country)
                .Include(e => e.Sections)
                .FirstOrDefault(e => e.Id == client.Id);

            if (currentClient != null)
            {
                currentClient.Fio = client.Fio;
                currentClient.Gender = client.Gender;
                currentClient.Email = client.Email;
                currentClient.CityId = client.CityId;
                currentClient.CountryId = client.CountryId;
                currentClient.Birthday = client.Birthday;
                currentClient.Sections = new List<Section>();

                var sectionIds = client.Sections.Select(e => e.Id);
                currentClient.Sections = db.Sections.Where(e => sectionIds.Contains(e.Id)).ToList();

                db.Clients.Update(currentClient);
                db.SaveChanges();
            }
        }
    }

    // ■ Обновление информации о странах;
    public void UpdateCountry(Country country)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var currentCountry = db.Countries
                .Include(e => e.Cities)
                .Include(e => e.Clients)
                .FirstOrDefault(e => e.Id == country.Id);

            if (currentCountry != null)
            {
                currentCountry.Name = country.Name;

                var cityIds = db.Cities.Select(e => e.Id);
                currentCountry.Cities = db.Cities.Where(e => cityIds.Contains(e.Id)).ToList();

                var clientIds = db.Clients.Select(e => e.Id);
                currentCountry.Cities = db.Cities.Where(e => clientIds.Contains(e.Id)).ToList();
                
                db.Countries.Update(currentCountry);
                db.SaveChanges();
            }
        }
    }

    // ■ Обновление информации о городах; 
    public void UpdateCity(City city)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Cities.Update(city);
            db.SaveChanges();
        }
    }
    
    // ■ Обновление информации о разделах; 
    public void UpdateSection(Section section)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var currentSection = db.Sections
                .Include(e => e.Clients)
                .Include(e => e.SaleProducts)
                .FirstOrDefault(e => e.Id == section.Id);

            if (currentSection != null)
            {
                currentSection.Name = section.Name;
                var saleProductIds = db.Cities.Select(e => e.Id);
                currentSection.SaleProducts = db.SaleProducts.Where(e => saleProductIds.Contains(e.Id)).ToList();

                var clientIds = db.Clients.Select(e => e.Id);
                currentSection.Clients = db.Clients.Where(e => clientIds.Contains(e.Id)).ToList();
                
                db.Sections.Update(currentSection);
                db.SaveChanges();
            }
        }
    }
    
    // ■ Обновление информации об аукционных товарах.
    public void UpdateSaleProduct(SaleProduct saleProduct)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var currentSaleProduct = db.SaleProducts
                .Include(e => e.Country)
                .Include(e => e.Sections)
                .FirstOrDefault(e => e.Id == saleProduct.Id);

            if (currentSaleProduct != null)
            {
                currentSaleProduct.Name = saleProduct.Name;
                currentSaleProduct.CountryId = saleProduct.CountryId;
                currentSaleProduct.StartSale = saleProduct.StartSale;
                currentSaleProduct.EndSale = saleProduct.EndSale;

                var sectionIds = db.Sections.Select(e => e.Id);
                currentSaleProduct.Sections = db.Sections.Where(e => sectionIds.Contains(e.Id)).ToList();

                db.SaleProducts.Update(currentSaleProduct);
                db.SaveChanges();
            }
        }
    }
    
    // ■ Удаление информации о покупателях; 
    public void RemoveClient(Client client)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Clients.Remove(client);
            db.SaveChanges();
        }
    }
    
    // ■ Удаление информации о странах; 
    public void RemoveCountry(Country country)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Countries.Remove(country);
            db.SaveChanges();
        }
    }
    
    // ■ Удаление информации о городах; 
    public void RemoveCity(City city)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Cities.Remove(city);
            db.SaveChanges();
        }
    }
    
    // ■ Удаление информации о разделах; 
    public void RemoveSection(Section section)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Sections.Remove(section);
            db.SaveChanges();
        }
    }
    
    // ■ Удаление информации об аукционных товарах.
    public void RemoveSaleProduct(SaleProduct saleProduct)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.SaleProducts.Remove(saleProduct);
            db.SaveChanges();
        }
    }
}