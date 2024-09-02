using HW_3_dop_1;
using Microsoft.EntityFrameworkCore;
using Task = HW_3_dop_1.Task;
using TaskStatus = HW_3_dop_1.TaskStatus;

public class ApplicationContext : DbContext
{
    public DbSet<Task> Tasks { get; set; } = null!;
    public DbSet<TaskStatus> TaskStatus { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Person> Persons { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectingString = "Server=localhost;Database=Tasks;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectingString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Armar" },
            new Project { Id = 2, Name = "LB" }
        );

        modelBuilder.Entity<TaskStatus>().HasData(
            new TaskStatus { Id = 1, Name = "Done" },
            new TaskStatus { Id = 2, Name = "In Progress" },
            new TaskStatus { Id = 3, Name = "Check" }
        );

        modelBuilder.Entity<Task>(e =>
        {
            // 	1) Укажите ограничения на длину текстовых полей (название, описание).
            e.Property(e => e.Name).HasMaxLength(60);
            e.Property(e => e.Description).HasMaxLength(255);
            // 2) Укажите ограничения на статус задачи (используйте Enum). 
            e.ToTable(e => e.HasCheckConstraint("CHK_StatusId", "StatusId IN (1,2,3)"));
            // 3) Добавьте индекс на столбец с датой создания задачи для оптимизации запросов.
            e.HasIndex(e => e.CreatedAt);
            // 4) Укажите ограничения и уникальные индексы для таблицы задач для обеспечения целостности данных.
            // 6) Убедитесь, что название задачи уникально в пределах таблицы Tasks, чтобы не было двух задач с одинаковым названием.
            e.HasIndex(e => e.Name).IsUnique();
            e.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            // 5) Добавьте проверку, чтобы дата дедлайна задачи была больше или равна дате создания задачи.
            e.ToTable(e => e.HasCheckConstraint("Deadline", "Deadline >= CreatedAt"));
            // Перед созданием базы данных, снабдите ее начальными данными, используя метод «HasData» Fluent Api. 
            e.HasData(
                new Task
                {
                    Id = 1,
                    Name = "FirstTask",
                    Description = "description",
                    StatusId = 1,
                    ProjectId = 1,
                },
                new Task
                {
                    Id = 2,
                    Name = "Add header",
                    Description = "add header for main block",
                    StatusId = 2,
                    ProjectId = 2,
                },
                new Task
                {
                    Id = 3,
                    Name = "Fix styles",
                    Description = "Fix styles in footer",
                    StatusId = 1,
                    ProjectId = 2,
                    CreatedAt = DateTime.Now
                }
            );
        });
    }
}