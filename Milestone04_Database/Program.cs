using Milestone04_Database.Data;
using Milestone04_Database.Presentation;
using Milestone04_Database.Repositories;
using Milestone04_Database.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var database = new DatabaseContext("library.db");
database.Initialize();

var userRepository = new UserRepository(database);
var genreRepository = new GenreRepository(database);
var bookRepository = new BookRepository(database);

var authService = new AuthService(userRepository);
var bookService = new BookService(bookRepository, genreRepository, authService);

var ui = new ConsoleUI(authService, bookService);
ui.Run();