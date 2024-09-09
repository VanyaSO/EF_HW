using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

namespace HW_6;
// Разработайте систему управления студентами и курсами для университета.
// В системе есть информация о студентах, курсах, преподавателях и результате их обучения.

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            
            List<Instructor> instructors = new List<Instructor>
            {
                new Instructor { FirstName = "John", LastName = "Doe" },
                new Instructor { FirstName = "Jane", LastName = "Smith" },
                new Instructor { FirstName = "Mark", LastName = "Johnson" }
            };
            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Ivan", LastName = "Ivanov", Birthday = new DateTime(2001, 5, 20) },
                new Student { FirstName = "Gleb", LastName = "Petrov", Birthday = new DateTime(1995, 8, 15) },
                new Student { FirstName = "Alex", LastName = "Sidorov", Birthday = new DateTime(1999, 1, 10) },
                new Student { FirstName = "Maria", LastName = "Alinov", Birthday = new DateTime(2002, 11, 5) },
                new Student { FirstName = "Sergey", LastName = "Smirnov", Birthday = new DateTime(1998, 4, 22) },
                new Student { FirstName = "Leha", LastName = "Petvik", Birthday = new DateTime(1998, 4, 22) },
                new Student { FirstName = "Sasha", LastName = "Miler", Birthday = new DateTime(1990, 1, 17) }
            };
            List<Course> courses = new List<Course>
            {
                new Course { Title = "Mathematics", Description = "Basic math course", Instructor = instructors[0], Students = new List<Student>{students[0], students[2]}},
                new Course { Title = "Physics", Description = "Intro to physics", Instructor = instructors[1], Students = new List<Student>{students[0], students[1], students[2], students[3], students[4], students[5]} },
                new Course { Title = "Chemistry", Description = "Basic chemistry course", Instructor = instructors[2], Students = new List<Student>{students[4]} },
                new Course { Title = "Biology", Description = "Introduction to biology", Instructor = instructors[0] },
                new Course { Title = "History", Description = "World history overview", Instructor = instructors[1], Students = new List<Student>{students[1], students[3]} }
            };
            db.Instructors.AddRange(instructors);
            db.Students.AddRange(students);
            db.Courses.AddRange(courses);
            db.SaveChanges();
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            // Используя методы расширения LINQ TO ENTITIES, выполните следующие запросы:

            // 1) Получить список студентов, зачисленных на определенный курс.
            var task1 = db.Enrollments
                .Where(e => e.Course.Title == "Mathematics")
                .Select(e => e.Student).ToList();

            // 2) Получить список курсов, на которых учит определенный преподаватель.
            var task2 = db.Courses
                .Where(e => e.Instructor.FirstName == "John")
                .ToList();
            
            // 3) Получить список курсов, на которых учит определенный преподаватель, вместе с именами студентов, зачисленных на каждый курс.
            var task3 = db.Courses
                .Where(e => e.Instructor.FirstName == "John")
                .Include(e => e.Students)
                .Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title,
                    StudentNames = e.Students
                        .Select(s => new
                        {
                            Id = s.Id,
                            FullName = s.FirstName + " " + s.LastName
                        })
                        .ToList()
                })
                .ToList();

            // 4) Получить список курсов, на которые зачислено более 5 студентов.
            var task4 = db.Courses
                .Where(c => c.Students.Count > 5)
                .ToList();

            // 5) Получить список студентов, старше 25 лет.
            var task5 = db.Students
                .Where(s => DateTime.Now.Year - s.Birthday.Year > 25)
                .ToList();

            // 6) Получить средний возраст всех студентов.
            double task6 = db.Students.Average(s=> DateTime.Now.Year - s.Birthday.Year);

            // 7) Получить самого молодого студента.
            var task7 = db.Students
                .OrderByDescending(s => s.Birthday)
                .FirstOrDefault();

            // 8) Получить количество курсов, на которых учится студент с определенным Id.
            var task8 = db.Enrollments
                .Where(e => e.StudentId == 1)
                .GroupBy(e => e.CourseId)
                .Select(g => new { CountCourses = g.Count() })
                .Count();


            // 9) Получить список имен всех студентов.
            var task9 = db.Students
                .Select(e => new { Name = e.FirstName })
                .ToList();

            // 10) Сгруппировать студентов по возрасту.
            var task10 = db.Students
                .OrderBy(s => s.Birthday)
                .ToList();
            
            // 11) Получить список студентов, отсортированных по фамилии в алфавитном порядке.
            var task11 = db.Students
                .OrderBy(s => s.LastName)
                .ToList();
            
            // 12) Получить список студентов вместе с информацией о зачислениях на курсы.
            var task12 = db.Enrollments
                .Include(e => e.Student)
                .Select(e => new
                {
                    Student = e.Student,
                    EnrollmentDate = e.Date
                })
                .ToList();
            
            // 13) Получить список студентов, не зачисленных на определенный курс.
            var task13 = db.Students
                .Where(s => !db.Enrollments.Any(e => e.CourseId == 2 && e.StudentId == s.Id))
                .ToList();

            // 14) Получить список студентов, зачисленных одновременно на два определенных курса.
            var task14 = db.Enrollments
                .Where(e => e.CourseId == 1 || e.CourseId == 2)
                .GroupBy(e => e.StudentId)
                .Where(g => g.Count() == 2)
                .Select(g => g.FirstOrDefault().Student)
                .ToList();
            
            // 15) Получить количество студентов на каждом курсе.
            var task15 = db.Enrollments
                .Include(e => e.Course)
                .GroupBy(e => e.CourseId)
                .Select(g => new
                {
                    CourseName = g.FirstOrDefault().Course.Title,
                    Count = g.Count()
                })
                .ToList();
        }
    }
};

public class ApplicationContext : DbContext
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<Instructor> Instructors { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=HW;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasMany(s => s.Courses).WithMany(c => c.Students).UsingEntity<Enrollment>(
            e => e.HasOne(e => e.Course).WithMany(g => g.Enrollments).HasForeignKey(e => e.CourseId),
            e => e.HasOne(e => e.Student).WithMany(s => s.Enrollments).HasForeignKey(e => e.StudentId),
            e =>
            {
                e.ToTable("Enrollments");
            }
        );

        modelBuilder.Entity<Enrollment>().Property(e => e.Date).HasDefaultValueSql("GETDATE()");
    }
}


// Определите следующие классы:
// ■ Student (Студент): Информация о студентах, включая их идентификатор, имя, фамилию и дату рождения.
public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public List<Course> Courses { get; set; }
    public List<Enrollment> Enrollments { get; set; }
}

// ■ Course (Курс): Информация о курсах, включая их идентификатор, название и описание.
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<Student> Students { get; set; }
    public List<Enrollment> Enrollments { get; set; }
    public int InstructorId { get; set; }
    
    public Instructor Instructor { get; set; }
}

// ■ Enrollment (Зачисление): Информация о зачислении студентов на курсы, включая идентификатор зачисления,
// идентификатор студента, идентификатор курса и дату зачисления.
public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; }
    
    public Student Student { get; set; }
    public Course Course { get; set; }
}

// ■ Instructor (Преподаватель): Информация о преподавателях, включая их идентификатор, имя и фамилию.
public class Instructor
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Course> Courses { get; set; }
}