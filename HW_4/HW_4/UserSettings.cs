namespace HW_4;

public class UserSettings
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Passport { get; set; }
    public int Age { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}