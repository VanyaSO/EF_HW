namespace HW_3;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool isBlocked { get; set; }
    public DateTime? DateTimeBlocked { get; set; }
}