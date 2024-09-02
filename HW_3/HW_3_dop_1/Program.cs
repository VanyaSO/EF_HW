﻿namespace HW_3_dop_1;

// Разработайте систему управления задачами для команды разработки программного обеспечения. Система должна позволять создавать, управлять и отслеживать выполнением задач.
// Создайте таблицу «Tasks» для хранения информации о задачах.
// Используя Fluent Api, настройте вашу таблицу, следующим образом:
// 1) Укажите ограничения на длину текстовых полей (название, описание).
// 2) Укажите ограничения на статус задачи (используйте Enum).
// 3) Добавьте индекс на столбец с датой создания задачи для оптимизации запросов.
// 4) Укажите ограничения и уникальные индексы для таблицы задач для обеспечения целостности данных.
// 5) Добавьте проверку, чтобы дата дедлайна задачи была больше или равна дате создания задачи.
// 6) Убедитесь, что название задачи уникально в пределах таблицы Tasks, чтобы не было двух задач с одинаковым названием.
// Перед созданием базы данных, снабдите ее начальными данными, используя метод «HasData» Fluent Api. 
// Проверьте корректности работы вашей программы.

    
class Program
{
    static void Main(string[] args)
    {
        TaskService taskService = new TaskService();
        taskService.EnsurePopulate();

        taskService.AddPerson(new Person{ FullName = "Ivan Ushachov" });
        Task? task = taskService.GetTask(1);
        if(task != null)
        {
            task.Person = taskService.GetPerson(1);
            taskService.UpdateTask(task);
        }
        // и тд...
    }
}