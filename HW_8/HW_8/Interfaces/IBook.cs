using HW_8.Models;

namespace HW_8.Interfaces;

public interface IBook
{
    Task<IEnumerable<Book>> GetAllBooksAsync();     
    Task<IEnumerable<Book>> GetAllBooksWithAuthorsAndCategoryAsync();

    Task<Book> GetBookAsync(int id);
    Task<IEnumerable<Book>> GetBooksByNameAsync(string name);
    Task<Book> GetBooksWithAuthorsAndReviewAndCategoryAsync(int id);

    Task AddBookAsync(Book book);
    Task DeleteBookAsync(Book book);
    Task EditBookAsync(Book book);
}
