// Опишите таблицу: «Поезда» (минимум 6 столбцов). 
// Создайте соответствующий класс.
// Выполните: добавление, получение, редактирование и удаление данных из таблицы. Каждая операция, в отдельном методе.
// Для соединения с базой данных, используйте файл конфигурации .json.

using hw_1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace En;

class Program
{
    static async Task Main(string[] args)
    {
        DatabaseService dbService = new DatabaseService();
        await dbService.EnsurePopulateTrains();

        // добавление
        Train train = new Train
        {
            Type = "Passenger", Model = "VVS", Manufacture = "VVS", ManufactureYear = "2024",
            Capacity = 1000, MaxSpeed = 522
        };
        await dbService.AddTrain(train);

        // получение
        Train getTrain = await dbService.GetTrain(2);
        if (getTrain != null)
        {
            // редактирование
            getTrain.Capacity = 2100;
            dbService.UpdateTrain(getTrain);
        }
        
        // удаление
        dbService.DeleteTrain(await dbService.GetTrain(1));
        
        
        // доп задания 1
        // Описать класс “Меню Блюд”. Используя 2 отдельных using, добавить данные в таблицу, как один объект, так и коллекцию.
        // Считать из таблицы информацию в коллекцию, проверить перед этим доступность базы данных.
        // Получить все блюда, в названии которых содержится слово “Суп”. Получить блюдо по Id.
        // Получить самое последнее блюдо из таблицы.
        dbService.EnsurePopulateDishes();
        if (dbService.IsMenuDishesDatabaseAvailable())
        {
            dbService.AddDish(new Dish
            {
                Name = "Суп с морепродуктами",
                Description = "description",
                Price = 299
            });
            
            dbService.AddRangeDish(new Dish
            {
                Name = "Куриный суп",
                Description = "description",
                Price = 169
            },
            new Dish
            {
                Name = "Плов",
                Description = "description",
                Price = 99
            });
            
            var menuDishes = await dbService.GetDishes();
            if (menuDishes.Count!=0)
            {
                foreach (var dish in menuDishes)
                {
                    Console.Write(dish.Name);
                    Console.Write(dish.Price);
                    Console.WriteLine();
                }   
            }

            var dishById = await dbService.GetDishById(2);
            if (dishById != null)
            {
                Console.WriteLine(dishById);
            }

            List<Dish> menuDishes1 = await dbService.GetDishByName("Суп");
            if (menuDishes1.Count != 0)
            {
                foreach (var dish in menuDishes1)
                {
                    Console.Write(dish.Name);
                    Console.Write(dish.Price);
                    Console.WriteLine();
                }   
            }
        }
    }
}