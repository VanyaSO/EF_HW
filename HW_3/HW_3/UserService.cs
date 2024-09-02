using HW_3;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    public void EnsurePopulate()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            // db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.SaveChanges();
        }
    }

    public User? GetUser(string email)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            User user = db.Users.FirstOrDefault(e => e.Email == email);
            if (user != null && user.isBlocked)
                throw new Exception("Аккаунт временно заблокирован");

            return user;
        }
    }

    public void AddUser(User user)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
    }

    public bool IsRegistredUser(string email)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Users.Any(e => e.Email == email);
        }
    }

    public void BlockUser(User user)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            user.isBlocked = true;
            user.DateTimeBlocked = DateTime.Now;
            db.Users.Update(user);
            db.SaveChanges();
        }
    }

    public List<Book> GetBookList(int position, int take)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Books.OrderBy(b => b.Id).Skip(position).Take(take).ToList();
        }
    }

    public List<Book> GetBookByName(string name)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Books.Where(b => b.Name.Contains(name)).ToList();
        }
    }
}