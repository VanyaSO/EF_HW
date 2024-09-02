namespace HW_3_dop_1;

public class TaskService
{
    public void EnsurePopulate()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.SaveChanges();
        }
    }

    public void AddPerson(Person obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Persons.Add(obj);
            db.SaveChanges();
        }
    }
    public void AddProject(Project obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Projects.Add(obj);
            db.SaveChanges();
        }
    }
    public void AddStatus(TaskStatus obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.TaskStatus.Add(obj);
            db.SaveChanges();
        }
    }
    public void AddTask(Task obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Tasks.Add(obj);
            db.SaveChanges();
        }
    }

    public Person? GetPerson(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Persons.FirstOrDefault(e => e.Id == id);
        }
    }
    public Project? GetProject(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Projects.FirstOrDefault(e => e.Id == id);
        }
    }
    public TaskStatus? GetTaskStatus(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.TaskStatus.FirstOrDefault(e => e.Id == id);
        }
    }
    public Task? GetTask(int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Tasks.FirstOrDefault(e => e.Id == id);
        }
    }

    public void UpdatePerson(Person obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Persons.Update(obj);
            db.SaveChanges();
        }
    }
    public void UpdateProject(Project obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Projects.Update(obj);
            db.SaveChanges();
        }
    }
    public void UpdateTaskStatus(TaskStatus obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.TaskStatus.Update(obj);
            db.SaveChanges();
        }
    }
    public void UpdateTask(Task obj)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Tasks.Update(obj);
            db.SaveChanges();
        }
    }
}