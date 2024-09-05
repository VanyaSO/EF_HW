namespace hw_4_dop_1;

public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public List<Project> Projects { get; set; }
}