namespace HW_8.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Book> Books { get; set; }

    public override string ToString()
    {
        return String.Format("Name - {0}\nDescription - {1}", Name, Description);
    }
}
