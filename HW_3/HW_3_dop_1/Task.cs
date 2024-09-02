namespace HW_3_dop_1;

public class Task
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int StatusId { get; set; }
    public int ProjectId { get; set; }
    public int? PersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? Deadline { get; set; }

    public TaskStatus Status { get; set; }
    public Project Project { get; set; }
    public Person Person { get; set; }
}