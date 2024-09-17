using HW_8.Data;
using HW_8.Interfaces;
using HW_8.Migrations;
using HW_8.Models;
using HW_8.Repository;
using Microsoft.EntityFrameworkCore;

namespace HW_8;

public partial class Program
{
    public static ApplicationContext DbContext() => new ApplicationContextFactory().CreateDbContext();
    private static IAuthor _authors;
    private static ICategory _categories;
    private static IBook _books;
    private static IPromotion _promotions;
    private static IOrder _orders;
    private static IReview _reviews;
    
    
    enum ShopMenu
    {
        Books, Authors, Categories, Orders, SearchAuthors, SearchBooks, SearchCategories, SearchOrders, AddBook, AddAuthor, AddCategory, AddOrder, Exit
    }
    
    static async Task Main()
    {
        Initialize();
        int input = new int();

        do
        {
            input = ConsoleHelper.MultipleChoice(true, new ShopMenu());
            switch ((ShopMenu)input)
            {
                case ShopMenu.Books:
                    await ReviewBooks();
                    break;
                case ShopMenu.Authors:
                    await ReviewAuthors();
                    break;
                case ShopMenu.Categories:
                    await ReviewCategories();
                    break;
                case ShopMenu.Orders:
                    await ReviewOrders();
                    break;
                case ShopMenu.SearchAuthors:
                    await SearchAuthors();
                    break;
                case ShopMenu.SearchBooks:
                    await SearchBooks();
                    break;
                case ShopMenu.SearchCategories:
                    await SearchCategories();
                    break;
                case ShopMenu.SearchOrders:
                    await SearchOrders();
                    break;
                case ShopMenu.AddBook:
                    await AddBook();
                    break;
                case ShopMenu.AddAuthor:
                    await AddAuthor();
                    break;
                case ShopMenu.AddCategory:
                    await AddCategory();
                    break;
                case ShopMenu.AddOrder:
                    await AddOrder();
                    break;
                case ShopMenu.Exit:
                    break;
            }
            
            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        } while ((ShopMenu)input != ShopMenu.Exit);
    }

    static void Initialize()
    {
        new DbInit().Init(DbContext());
        _authors = new AuthorRepository();
        _categories = new CategoryRepository();
        _books = new BookRepository();
        _orders = new OrderRepository();
        _reviews = new ReviewRepository();
    }
};
