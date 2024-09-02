namespace HW_3;

public class Menu
{
    private readonly UserService _dbService = new UserService();
    private User? _user;
    private bool _isAuthenticated;
    
    public void StartMenu()
    {
        Console.WriteLine("Добро пожаловать");
        Console.WriteLine("1) Вход");
        Console.WriteLine("2) Регистрация");
        Console.WriteLine();

        string action = Console.ReadLine();
        
        switch (action)
        {
            case "1":
                try
                {
                    Login();
                    goto default;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    StartMenu();
                }
                break;
            case "2":
                try
                {
                    Register();
                    goto default;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    StartMenu();
                }
                break;
            default:
                if (_isAuthenticated)
                    MainMenu();
                else
                    StartMenu();
                break;
        }
    }
    public void MainMenu()
    {
        Console.WriteLine($"Приветствуем {_user?.Name}");
        Console.WriteLine("1) Просмотр книг");
        Console.WriteLine("2) Найти книгу");
        Console.WriteLine("0) Выйти");

        string action = Console.ReadLine();
        
        switch (action)
        {
            case "1":
                ViewBook();
                break;
            case "2":
                var books = FindBookByName();
                PrintBooks(books);
                goto default;
            case "0":
                _user = null;
                _isAuthenticated = false;
                StartMenu();
                break;
            default:
                MainMenu();
                break;
        }
    }
    
    
    private void Login()
    {
        string email, password;
        Console.WriteLine("Введите данные для входа");
        
        Console.WriteLine("Email:");
        email = Console.ReadLine();

        User user = _dbService.GetUser(email);
        if (user != null)
        {
            int loginAttemptCount = 0;
            while (loginAttemptCount < 3)
            {
                Console.WriteLine("Пароль:");
                password = Console.ReadLine();
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    _isAuthenticated = true;
                    _user = user;
                    return;
                }
                
                loginAttemptCount++;
            }

            _dbService.BlockUser(user);
            throw new Exception("Ваш аккаунт заблокирован из-за множества неудачных попыток входа");
        }
        
        throw new Exception("Пользователь не найден");
    }
    private void Register()
    {
        Console.WriteLine("Введите имя:");
        string name = Console.ReadLine();
        Console.WriteLine("Введите email:");
        string email = Console.ReadLine();
        Console.WriteLine("Введите пароль");
        string password = Console.ReadLine();

        User user = new User { Name = name, Email = email, Password = BCrypt.Net.BCrypt.HashPassword(password) };
        
        if (_dbService.IsRegistredUser(user.Email))
            throw new Exception("Пользователь существует");

        _dbService.AddUser(user);
        _user = user;
        _isAuthenticated = true;
    }

    private void ViewBook()
    {
        int position = 0;
        int take = 5;
        
        List<Book> books = _dbService.GetBookList(position, take);
        PrintBooks(books);
        
        while (true)
        {
            if(position > 0)
                Console.WriteLine("1) Назад");
            if (books.Count == take)
                Console.WriteLine("2) Врепед");

            Console.WriteLine("0) Вернутся назад");

            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    if(position > 0)
                        position -= 5;
                        goto default;
                case "2":
                    if (books.Count == take)
                        position += 5;
                        goto default;
                case "0":
                    MainMenu();
                    return;
                default:
                    books = _dbService.GetBookList(position, take);
                    PrintBooks(books);
                    break;
            }   
        }
    }

    private void PrintBooks(List<Book> books)
    {
        Console.WriteLine();
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Name} {book.Author}");
        }
    }

    private List<Book> FindBookByName()
    {
        Console.WriteLine("Введите название книги");
        string nameBook = Console.ReadLine();
        return _dbService.GetBookByName(nameBook);
    }
}