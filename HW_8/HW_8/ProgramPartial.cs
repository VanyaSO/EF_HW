using HW_8.Data;
using HW_8.Interfaces;
using HW_8.Migrations;
using HW_8.Models;
using HW_8.Repository;
using HW_8.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HW_8;

public partial class Program
{
    static async Task ReviewAuthors()
    {
        var allAuthors = await _authors.GetAllAuthorsAsync();
        var authors = allAuthors
            .Select(author => new ItemView { Id = author.Id, Value = author.Name })
            .ToList();
        
        int selectedAuthorId = ItemsHelper.MultipleChoice(true, authors, true);
        if (selectedAuthorId != 0)
        {
            var selectedAuthor = await _authors.GetAuthorAsync(selectedAuthorId);
            await AuthorInfo(selectedAuthor);
        }
    }
    static async Task AddAuthor()
    {
        Console.Clear();
        string authorName = InputHelper.GetString("author 'Name' with 'Surname'");
        await _authors.AddAuthorAsync(new Author { Name = authorName });
        Console.WriteLine("Author successfully added.");
    }
    static async Task AuthorInfo(Author author)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "Browse books" },
                new ItemView { Id = 2, Value = "Edit author" },
                new ItemView { Id = 3, Value = "Delete author" },
            }, IsMenu: true, message: String.Format("{0}\n", author), startY: 5, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                await ReviewAuthorBooks(author);
                break;
            case 2:
                await EditAuthor(author);
                break;
            case 3:
                await RemoveAuthor(author);
                break;
        }

        await ReviewAuthors();
    }
    static async Task ReviewAuthorBooks(Author author)
    {
        var allBooks = await _books.GetAllBooksWithAuthorsAndCategoryAsync();
        var authorBooks = allBooks
            .Where(e => e.Authors.Any(e => e.Name == author.Name)).ToList();

        if (authorBooks.Count > 0)
        {
            foreach (var book in authorBooks)
            {
                Console.WriteLine(book);
            }
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    static async Task RemoveAuthor(Author author)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "Yes" },
                new ItemView { Id = 0, Value = "No" }
            }, message: String.Format("Are you sure you want to delete the author {0}?\n", author.Name), startY: 2);

        if (result == 1)
        {
            await _authors.DeleteAuthorAsync(author);
            Console.WriteLine("The author has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task EditAuthor(Author author)
    {
        Console.WriteLine("Changing: {0}", author.Name);
        author.Name = InputHelper.GetString("author 'Name' with 'Surname'");
        await _authors.EditAuthorAsync(author);
        Console.WriteLine("Author successfully changed.");
    }
    static async Task SearchAuthors()
    {
        Console.Clear();
        string authorName = InputHelper.GetString("author name or surname");
        var currentAuthors = await _authors.GetAuthorsByNameAsync(authorName);
        
        if (currentAuthors.Count() > 0)
        {
            var authors = currentAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, authors, true);
            
            if (result != 0)
            {
                var currentAuthor = await _authors.GetAuthorAsync(result);
                await AuthorInfo(currentAuthor);
            }
        }
        else
        {
            Console.WriteLine("No authors were found by this attribute.");
        }
    }
    
    
    // categories
    static async Task ReviewCategories()
    {
        var allCategories = await _categories.GetAllCategoriesAsync();
        var categories = allCategories
            .Select(ct => new ItemView { Id = ct.Id, Value = ct.Name })
            .ToList();
        
        int selectedCategoryId = ItemsHelper.MultipleChoice(true, categories, true);
        if (selectedCategoryId != 0)
        {
            var selectedCategory = await _categories.GetCategoryAsync(selectedCategoryId);
            await CategoryInfo(selectedCategory);
        }
    } 
    static async Task AddCategory()
    {
        Console.Clear();
        string categoryName = InputHelper.GetString("category 'Name'");
        string categoryDescription= InputHelper.GetString("category 'Description'");
        
        await _categories.AddCategoryAsync(new Category() { Name = categoryName, Description = categoryDescription});
        Console.WriteLine("Category successfully added.");
    }
    static async Task CategoryInfo(Category category)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Browse books" },
            new ItemView { Id = 2, Value = "Edit category" },
            new ItemView { Id = 3, Value = "Delete category" },
        }, IsMenu: true, message: String.Format("{0}\n", category), startY: 5, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                await ReviewCategoryBooks(category);
                break;
            case 2:
                await EditCategory(category);
                break;
            case 3:
                await RemoveCategory(category);
                break;
        }

        await ReviewCategories();
    }
    static async Task ReviewCategoryBooks(Category category)
    {
        var allBooks = await _books.GetAllBooksWithAuthorsAndCategoryAsync();
        var categoryBooks = allBooks
            .Where(e => e.Category.Name == category.Name).ToList();

        if (categoryBooks.Count > 0)
        {
            foreach (var book in categoryBooks)
            {
                Console.WriteLine(book);
            }
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    static async Task EditCategory(Category category)
    {
        Console.WriteLine("Changing: {0}", category.Name);
        category.Name = InputHelper.GetString("category 'Name'");
        
        Console.WriteLine("Changing: {0}", category.Description);
        category.Description = InputHelper.GetString("category 'Description'");
        
        await _categories.UpdateCategoryAsync(category);
        Console.WriteLine("Category successfully changed.");
    }
    static async Task RemoveCategory(Category category)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: String.Format("Are you sure you want to delete the category {0}?\n", category.Name), startY: 2);

        if (result == 1)
        {
            await _categories.DeleteCategoryAsync(category);
            Console.WriteLine("The category has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task SearchCategories()
    {
        Console.Clear();
        string categoryName = InputHelper.GetString("category name");
        var currentCategories = await _categories.GetCategoriesByNameAsync(categoryName);
        
        if (currentCategories.Count() > 0)
        {
            var categories = currentCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, categories, true);
            
            if (result != 0)
            {
                var currentCategory = await _categories.GetCategoryAsync(result);
                await CategoryInfo(currentCategory);
            }
        }
        else
        {
            Console.WriteLine("No categories were found by this attribute.");
        }
    }
    
    // books
    static async Task ReviewBooks()
    {
        var allBooks = await _books.GetAllBooksAsync();
        var books = allBooks
            .Select(book => new ItemView { Id = book.Id, Value = book.Title })
            .ToList();
        
        int selectedBookId = ItemsHelper.MultipleChoice(true, books, true);
        if (selectedBookId != 0)
        {
            var selectedBook = await _books.GetBooksWithAuthorsAndReviewAndCategoryAsync(selectedBookId);
            await BookInfo(selectedBook);
        }
    } 
    static async Task AddBook()
    {
        Console.Clear();
        string title = InputHelper.GetString("book 'Name'");
        string description = InputHelper.GetString("book 'Description'");
        DateTime publishedOn = InputHelper.GetDate("book 'PublishedOn (yyyy-MM-dd)'").ToDateTime(TimeOnly.MinValue);

        string publisher = null;
        int isAddPublisher = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: "Add publisher ?", startY: 2);
        if (isAddPublisher != 0)
            publisher = InputHelper.GetString("book 'Publisher'");
        
        decimal price = InputHelper.GetDecimal("book 'Price'");

        Promotion promotion = null;
        int isAddPromotion = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: "Add Promotion ?", startY: 2);
        if (isAddPromotion != 0)
            promotion = CreatePromotion();
        
        
        var allAuthors = await _authors.GetAllAuthorsAsync();
        List<Author> bookAuthors = new List<Author>();
        int selectedAuthorId;
        while (true)
        {
            // выводим авторов которых мы еще не добавили
            var authors = allAuthors
                .Where(e => !bookAuthors.Select(e => e.Id).Contains(e.Id))
                .Select(e => new ItemView { Id = e.Id, Value = e.Name })
                .ToList();
            authors.Insert(0, new ItemView {Id = 0, Value = "Сontinue..."});
            
            selectedAuthorId = ItemsHelper.MultipleChoice(true, authors,
                message: $"Select author: ", startY: 5, optionsPerLine: 1);

            if (selectedAuthorId != 0)
            {
                bookAuthors.Add(await _authors.GetAuthorAsync(selectedAuthorId));
                Console.WriteLine("Author successfully add.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
                break;
        }


        List<Review> bookReviews = new List<Review>();
        int reviewAction;
        while (true)
        {
            reviewAction = ItemsHelper.MultipleChoice(true, new List<ItemView>
                {
                    new ItemView {Id = 1, Value = "Yes"},
                    new ItemView {Id = 0, Value = "No"}
                }, message: $"Create new review ?", startY: 5, optionsPerLine: 1);

            if (reviewAction != 0)
            {
                bookReviews.Add(CreateReview());
                Console.WriteLine("Review successfully add.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
                break;
        }
        
        var allCategories = await _categories.GetAllCategoriesAsync();
        int selectedCategoryId = ItemsHelper.MultipleChoice(true, 
            allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList(), 
            message: "Select 'Category' for book", 
            startY: 5, optionsPerLine: 1);
        
        await _books.AddBookAsync(new Book {Title = title, Description = description, PublishedOn = publishedOn, Publisher = publisher, Price = price, Promotion = promotion, Authors = bookAuthors, Reviews = bookReviews, CategoryId = selectedCategoryId});
        Console.WriteLine("Book successfully add.");
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }
    static async Task BookInfo(Book book)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Edit book" },
            new ItemView { Id = 2, Value = "Delete book" },
        }, IsMenu: true, message: String.Format("{0}\n", book), startY: 10, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                await EditBook(book);
                break;
            case 2:
                await RemoveBook(book);
                break;
        }

        await ReviewBooks();
    }
    static async Task EditBook(Book book)
    {
        Console.WriteLine("Changing: {0}", book.Title);
        book.Title = InputHelper.GetString("book 'Name'");
        
        Console.WriteLine("Changing: {0}", book.Description);
        book.Description = InputHelper.GetString("book 'Description'");
        
        Console.WriteLine("Changing: {0}", book.PublishedOn.ToString("yyyy-MM-dd"));
        book.PublishedOn = InputHelper.GetDate("book 'PublishedOn'").ToDateTime(TimeOnly.MinValue);
        
        int isEditPublisher = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: "Edit publisher ?", startY: 2);
        
        if (isEditPublisher != 0)
        {
            Console.WriteLine("Changing: {0}", book.Publisher ?? "not");
            book.Publisher = InputHelper.GetString("book 'Publisher'");
        }
        
        Console.WriteLine("Changing: {0}", book.Price);
        book.Price = InputHelper.GetDecimal("book 'Price'");
        
        int isEditPromotion = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: "Edit Promotion ?", startY: 2);
        if (isEditPromotion != 0)
        {
            Console.WriteLine("Changing: {0}", book.Promotion != null ? book.Promotion.ToString() : "not");
            if (book.Promotion is null)
            {
                book.Promotion = CreatePromotion();
            }
            else
            {
                EditPromotion(book.Promotion);
            }
        }
        
        
        var allAuthors = await _authors.GetAllAuthorsAsync();
        var authors = allAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
        authors.Insert(0, new ItemView {Id = 0, Value = "Сontinue..."});
        
        int selectedAuthorId;
        while (true)
        {
            string currentAuthorsMessage = "\nCurrent authors: ";
            foreach (var bAuthor in book.Authors)
                currentAuthorsMessage += $"{bAuthor.Name}, ";
        
            selectedAuthorId = ItemsHelper.MultipleChoice(true, authors,
                message: $"Changing: book authors \n{currentAuthorsMessage}", startY: 5, optionsPerLine: 1);
            
            if (selectedAuthorId != 0)
                EditAuthorsBook(book, selectedAuthorId);
            else            
                break;
        }

        int reviewAction, selectedReviewId;
        while (true)
        {
            var allReviews = await _reviews.GetAllReviewsAsync(book.Id);
            var reviews = allReviews.Select(e => new ItemView { Id = e.Id, Value = string.Concat(e.UserEmail, "-", e.Comment) }).ToList();
            reviews.Insert(0, new ItemView {Id = 0, Value = "Back..."});
            
            reviewAction = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView {Id = 0, Value = "Continue..."},
                new ItemView {Id = 1, Value = "Add"},
                new ItemView {Id = 2, Value = "Delete"}
            }, message: "What do you want to do with the reviews ?", startY: 5, optionsPerLine: 1);

            switch (reviewAction)
            {
                case 1:
                    Review newReview = CreateReview();
                    newReview.BookId = book.Id;
                    await _reviews.AddReviewAsync(newReview);
                    continue;
                case 2:
                    selectedReviewId = ItemsHelper.MultipleChoice(true, reviews, message: "Select review for remove ?", startY: 5, optionsPerLine: 1);
                    if (selectedReviewId != 0)
                        await _reviews.DeleteReviewAsync(book.Reviews.FirstOrDefault(e => e.Id == selectedReviewId));
                    continue;
                case 0:
                    break;
            }
            break;
        }
        
        var allCategories = await _categories.GetAllCategoriesAsync();
        int selectedCategoryId = ItemsHelper.MultipleChoice(true, 
            allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList(), 
            message: String.Format("Select new 'Category' for book \nChange: {0}", book.Category.Name), 
            startY: 5, optionsPerLine: 1);
        
        book.CategoryId = selectedCategoryId;
        book.Category = await _categories.GetCategoryAsync(selectedCategoryId);
        
        await _books.EditBookAsync(book);
        Console.WriteLine("Book successfully changed.");
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }
    static Promotion CreatePromotion()
    {
        string name = InputHelper.GetString("promotion 'Name'");
        int variantPromotion = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 0, Value = "Percent" },
            new ItemView { Id = 1, Value = "Amount" }
        }, message: "Select variant promotion", startY: 2);

        decimal? percent = null;
        decimal? amount = null;
        switch (variantPromotion)
        {
            case 0:
                percent = InputHelper.GetDecimal("promotion percent:");
                break;
            case 1:
                amount = InputHelper.GetDecimal("promotion amount:");
                break;
        }

        return new Promotion { Name = name, Percent = percent, Amount = amount };
    }
    static Review CreateReview()
    {
        string userName = InputHelper.GetString("review 'user name'"); 
        string userEmail = InputHelper.GetString("review 'user email'"); 
        string comment = InputHelper.GetString("review 'comment'");
        byte stars = (byte)InputHelper.GetInt("review 'stars'");
        return new Review {UserName = userName, UserEmail = userEmail, Comment = comment, Stars = stars};
    }
    static async Task EditPromotion(Promotion promotion)
    {
        Console.WriteLine("Changing: {0}", promotion.Name);
        promotion.Name = InputHelper.GetString("promotion 'Name'");

        if (promotion.Percent is not null)
        {
            Console.WriteLine("Changing: {0}", promotion.Percent);
            promotion.Percent = InputHelper.GetDecimal("promotion 'Percent'");
        }
        else if (promotion.Amount is not null)
        {
            Console.WriteLine("Changing: {0}", promotion.Amount);
            promotion.Amount = InputHelper.GetDecimal("promotion 'Amount'");
        }
    }
    static async Task EditAuthorsBook(Book book, int authorId)
    {
        List <ItemView> menuForAuthor =  new List<ItemView>();
        if (!book.Authors.Select(e => e.Id).Contains(authorId))
        {
            menuForAuthor.Add(new ItemView {Id = 1, Value = "Add Author"});
        }
        else
        {
            menuForAuthor.Add(new ItemView {Id = 2, Value = "Remove Author"});
        }
        
        int result = ItemsHelper.MultipleChoice(true, menuForAuthor, message:"Select action", IsMenu: true, startY: 3, optionsPerLine: 1);
        Author selectedAuthor;
        switch (result)
        {
            case 0:
                return;
            case 1:
                selectedAuthor = await _authors.GetAuthorWhithBooksAsync(authorId);
                if (selectedAuthor is not null)
                {
                    book.Authors.Add(selectedAuthor);
                }
                break;
            case 2:
                selectedAuthor = book.Authors.FirstOrDefault(e => e.Id == authorId);
                book.Authors.Remove(selectedAuthor);
                break;
        }
    }
    static async Task RemoveBook(Book book)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: String.Format("Are you sure you want to delete the book {0}?\n", book.Title), startY: 2);

        if (result == 1)
        {
            await _books.DeleteBookAsync(book);
            Console.WriteLine("The book has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task SearchBooks()
    {
        Console.Clear();
        string bookName = InputHelper.GetString("book name");
        var currentBooks = await _books.GetBooksByNameAsync(bookName);
        
        if (currentBooks.Count() > 0)
        {
            var books = currentBooks.Select(e => new ItemView { Id = e.Id, Value = e.Title }).ToList();
            int result = ItemsHelper.MultipleChoice(true, books, true);
            
            if (result != 0)
            {
                var currentBook = await _books.GetBooksWithAuthorsAndReviewAndCategoryAsync(result);
                await BookInfo(currentBook);
            }
        }
        else
        {
            Console.WriteLine("No books were found by this attribute.");
        }
    }
    
    // orders
    static async Task ReviewOrders()
    {
        var allOrders = await _orders.GetAllOrdersAsync();
        var orders = allOrders
            .Select(order => new ItemView { Id = order.Id, Value = string.Concat(order.Id, "-", order.Address) })
            .ToList();
        
        int selectedOrderId = ItemsHelper.MultipleChoice(true, orders, true, optionsPerLine:1);
        if (selectedOrderId != 0)
        {
            var selectedOrder = await _orders.GetOrderWithOrderLinesAndBooksAsync(selectedOrderId);
            await OrderInfo(selectedOrder);
        }
    } 
    static async Task AddOrder()
    {
        Console.Clear();
        string customerName = InputHelper.GetString("order 'CustomerName'");
        string city = InputHelper.GetString("order 'City'");
        string address = InputHelper.GetString("order 'Address'");
        bool shipped = InputHelper.GetBoolean("order 'Shipped'");
        
        

        List<OrderLine> lines = new List<OrderLine>();
        var allBooks = await _books.GetAllBooksAsync();
        Dictionary<Book, int> addedBooks = new Dictionary<Book, int>();
        int selectedBookId;
        while (true)
        {
            // выводим книги которые мы еще не добавили
            var books = allBooks
                .Where(e => !lines.Select(l => l.BookId).Contains(e.Id))
                .Select(e => new ItemView { Id = e.Id, Value = e.Title })
                .ToList();
            books.Insert(0, new ItemView {Id = 0, Value = "Сontinue..."});
            
            selectedBookId = ItemsHelper.MultipleChoice(true, books, message: $"Select book: ", startY: 5, optionsPerLine: 1);

            if (selectedBookId != 0)
            {
                int quantity = InputHelper.GetInt("order 'Quantity'");
                lines.Add(new OrderLine {BookId = selectedBookId, Quantity = quantity});
                
                Console.WriteLine("Book successfully add.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                await _orders.AddOrderAsync(new Order
                    { CustomerName = customerName, City = city, Address = address, Shipped = shipped, Lines = lines });
                break;
            }
        }

        Console.WriteLine("Order successfully added.");
    }
    static async Task OrderInfo(Order order)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Edit order" },
            new ItemView { Id = 2, Value = "Delete order" },
        }, IsMenu: true, message: String.Format("{0}\n", order), startY: 10, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                await EditOrder(order);
                break;
            case 2:
                await RemoveOrder(order);
                break;
        }

        await ReviewOrders();
    }
    static async Task EditOrder(Order order)
    {
        Console.WriteLine("Changing: {0}", order.CustomerName);
        order.CustomerName = InputHelper.GetString("order 'CustomerName'");
        
        Console.WriteLine("Changing: {0}", order.City);
        order.City = InputHelper.GetString("order 'City'");
        
        Console.WriteLine("Changing: {0}", order.Address);
        order.Address = InputHelper.GetString("order 'Address'");
        
        Console.WriteLine("Changing: {0}", order.Shipped);
        order.Shipped = InputHelper.GetBoolean("order 'Shipped'");

        foreach (var line in order.Lines)
        {
            Console.WriteLine("Changing quantity: Book - {0}, Quantity - {1}", line.Book.Title, line.Quantity);
            line.Quantity = InputHelper.GetInt($"for {line.Book.Title} 'Quantity'");
            if (line.Quantity == 0)
                order.Lines.Remove(line);
        }
        
        await _orders.UpdateOrderAsync(order);
        Console.WriteLine("Order successfully changed.");
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }
    static async Task RemoveOrder(Order order)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes" },
            new ItemView { Id = 0, Value = "No" }
        }, message: String.Format("Are you sure you want to delete the order {0}?\n", order.Id), startY: 2);

        if (result == 1)
        {
            await _orders.DeleteOrderAsync(order);
            Console.WriteLine("The order has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task SearchOrders()
    {
        Console.Clear();
        string orderAddress = InputHelper.GetString("order address");
        var currentOrders = await _orders.GetAllOrdersByAddressAsync(orderAddress);
        
        if (currentOrders.Count() > 0)
        {
            var orders = currentOrders.Select(e => new ItemView { Id = e.Id, Value = string.Concat(e.Id, "-", e.Address) }).ToList();
            int result = ItemsHelper.MultipleChoice(true, orders, true, optionsPerLine:1);
            
            if (result != 0)
            {
                var currentOrder = await _orders.GetOrderWithOrderLinesAndBooksAsync(result);
                await OrderInfo(currentOrder);
            }
        }
        else
        {
            Console.WriteLine("No orders were found by this attribute.");
        }
    }
};
