using Microsoft.EntityFrameworkCore;

namespace HW_dop_2;

public class DbService
{
    // Добавление
    public void Add(params User[] users)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.AddRange(users);
            db.SaveChanges();
        }
    }
    public void Add(params Company[] companies)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Companies.AddRange(companies);
            db.SaveChanges();
        }
    }
    // Редактирование
    public void Update(User user)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.Update(user);
            db.SaveChanges();
        }
    }
    public void Update(Company company)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Companies.Update(company);
            db.SaveChanges();
        }
    }
    // Удаление
    public void RemoveUser(User user)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
    public void Remove(Company company)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Companies.Remove(company);
            db.SaveChanges();
        }
    }
    // Поиск
    public User? FindUser(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Users.Include(u => u.Company).FirstOrDefault(e => e.Id == id);
        }
    }
    public Company? FindCompany(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Companies.Include(c => c.Users).FirstOrDefault(e => e.Id == id);
        }
    }
}