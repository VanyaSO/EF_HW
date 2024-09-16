namespace HW_8.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PublishedOn { get; set; }
    public string? Publisher { get; set; }
    public decimal Price { get; set; }
    public Promotion? Promotion { get; set; }

    //Связи с другими классами
    public virtual ICollection<Author> Authors { get; set; }
    public virtual ICollection<Review> Reviews { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public override string ToString()
    {
        string authorsList = Authors.Count > 0
            ? string.Join(", ", Authors.Select(e => e.Name))
            : "No authors";

        string reviewsList = Reviews.Count > 0 ? 
            string.Join(", ", Reviews.Select(e => $"Stars: {e.Stars} Email: {e.UserEmail}, Comment: {e.Comment}")) : "No reviews";

        return String.Format("Title - {0}\nDescription - {1}\nCategory - {2}\nPublishedOn - {3}\nPublisher - {4}\nPrice - {5}\nAuthors - {6}\nReviews - {7}",
            Title, Description, Category.Name, PublishedOn.ToShortDateString(),
            Publisher, Price, authorsList, reviewsList);
    } 

}
