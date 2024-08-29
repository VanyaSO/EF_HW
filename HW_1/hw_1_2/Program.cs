using Microsoft.EntityFrameworkCore;

namespace hw_1_2;

// Выполнить “Reverse Engineering” для любой из созданных ранее баз данных.
// Получить все данные из любой таблицы в коллекцию и отобразить ее.
// Добавить в любую таблицу несколько объектов и после отобразить все данные из таблицы.

class Program
{
    
    static async Task Main(string[] args)
    {
        using (TrainsContext db = new TrainsContext())
        {
            List<Train> list = await db.Trains.ToListAsync();
            foreach (var train in list)
            {
                Console.WriteLine($"{train.Id} {train.Model}");
            }
            
            db.Trains.AddRange(new Train
            {
                Type = "New", Model = "New", Manufacture = "New", ManufactureYear = "0000",
                Capacity = 0, MaxSpeed = 0
            },
            new Train
            {
                Type = "New1", Model = "New1", Manufacture = "New1", ManufactureYear = "0000",
                Capacity = 0, MaxSpeed = 0
            });
            await db.SaveChangesAsync();
            
            List<Train> listNew = await db.Trains.ToListAsync();
            foreach (var train in listNew)
            {
                Console.WriteLine($"{train.Id} {train.Model}");
            }
        }
    }
}