using Milestone03_LayeredConsole.Presentation;
using Milestone03_LayeredConsole.Repositories;
using Milestone03_LayeredConsole.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var userRepository = new UserRepository("users.json");
var bookRepository = new BookRepository("books.json");

var authService = new AuthService(userRepository);
var bookService = new BookService(bookRepository, authService);

var ui = new ConsoleUI(authService, bookService);
ui.Run();