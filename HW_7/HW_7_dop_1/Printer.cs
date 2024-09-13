namespace HW_7_dop_1;

public static class Printer
{
    // Отображение всех покупателей; 
    public static void PrintClients(ICollection<Client> clients)
    {
        if (clients.Count > 0)
        {
            foreach (var client in clients)
            {
                Console.WriteLine($"{client.Id} {client.Fio} {client.Gender} {client.Birthday} {client.Email} {client.Country?.Name} {client.City?.Name}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    
    // ■ Отображение email всех покупателей; 
    public static void PrintEmailClients(ICollection<Client> clients)
    {
        if (clients.Count > 0)
        {
            foreach (var client in clients)
            {
                Console.WriteLine($"{client.Email}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    // ■ Отображение списка разделов; 
    public static void PrintSections(ICollection<Section> sections)
    {
        if (sections.Count > 0)
        {
            foreach (var section in sections)
            {
                Console.WriteLine($"{section.Id} {section.Name}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    
    // ■ Отображение списка аукционных товаров; 
    public static void PrintSaleProducts(ICollection<SaleProduct> saleProducts)
    {
        if (saleProducts.Count > 0)
        {
            foreach (var product in saleProducts)
            {
                Console.WriteLine($"{product.Id} {product.Name} {product.StartSale} {product.EndSale}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    
    // ■ Отображение всех городов; 
    public static void PrintCities(ICollection<City> cities)
    {
        if (cities.Count > 0)
        {
            foreach (var city in cities)
            {
                Console.WriteLine($"{city.Id} {city.Name}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    
    // ■ Отображение всех стран.
    public static void PrintCountries(ICollection<Country> countries)
    {
        if (countries.Count > 0)
        {
            foreach (var country in countries)
            {
                Console.WriteLine($"{country.Id} {country.Name}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    
}