// Milestone 02 - OOP and Persistence

Console.OutputEncoding = System.Text.Encoding.UTF8;

var library = new Library("books.json");
bool running = true;

while (running)
{
    ShowMenu();
    string option = Console.ReadLine() ?? "";

    switch (option)
    {
        case "1":
            AddBook();
            break;
        case "2":
            ListBooks();
            break;
        case "3":
            FindBooks();
            break;
        case "4":
            DeleteBook();
            break;
        case "0":
            running = false;
            Console.WriteLine("Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}

// Menu

void ShowMenu()
{
    Console.WriteLine();
    Console.WriteLine(" ━━━━━━━━━━━━━━━━━━━━━");
    Console.WriteLine("    PERSONAL LIBRARY");
    Console.WriteLine(" ━━━━━━━━━━━━━━━━━━━━━");
    Console.WriteLine("1. Add book");
    Console.WriteLine("2. List books");
    Console.WriteLine("3. Find book");
    Console.WriteLine("4. Delete book");
    Console.WriteLine("0. Exit\n");
    Console.Write("Choose an option: ");
}

// Main operations

void AddBook()
{
    PrintTitle("Add book");

    string title = ReadUniqueTitle();
    string author = ReadNonEmpty("Author: ");
    int year = ReadYear("Year: ");
    ReadingStatus status = ReadStatus();

    library.Add(title, author, year, status);
    Console.WriteLine("Book added successfully.");
}

void ListBooks()
{
    PrintTitle("All books");

    if (library.Books.Count == 0)
    {
        Console.WriteLine("There are no books yet.");
        return;
    }

    foreach (Book book in library.Books)
    {
        Console.WriteLine(book);
    }
}

void FindBooks()
{
    PrintTitle("Find book");
    string text = ReadNonEmpty("Search by title or author: ");

    List<Book> results = library.Find(text);

    if (results.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    foreach (Book book in results)
    {
        Console.WriteLine(book);
    }
}

void DeleteBook()
{
    PrintTitle("Delete book");

    if (library.Books.Count == 0)
    {
        Console.WriteLine("There are no books to delete.");
        return;
    }

    Console.Write("Enter the Id of the book to delete: ");
    string input = Console.ReadLine() ?? "";

    if (int.TryParse(input, out int id) && library.Remove(id))
    {
        Console.WriteLine("Book deleted.");
    }
    else
    {
        Console.WriteLine("No book with that Id.");
    }
}

// Aux methods

void PrintTitle(string text)
{
    Console.WriteLine();
    Console.WriteLine($"==== {text} ====");
}

string ReadNonEmpty(string message)
{
    while (true)
    {
        Console.Write(message);
        string input = (Console.ReadLine() ?? "").Trim();

        if (input != "")
        {
            return input;
        }
        Console.WriteLine("This field cannot be empty.");
    }
}

string ReadUniqueTitle()
{
    while (true)
    {
        string title = ReadNonEmpty("Title: ");

        if (!library.TitleExists(title))
        {
            return title;
        }
        Console.WriteLine("A book with that title already exists. Enter a different title.");
    }
}

int ReadYear(string message)
{
    while (true)
    {
        Console.Write(message);
        string input = Console.ReadLine() ?? "";

        if (int.TryParse(input, out int year) && year >= 1 && year <= DateTime.Now.Year)
        {
            return year;
        }
        Console.WriteLine($"Enter a valid year (1 - {DateTime.Now.Year}).");
    }
}

ReadingStatus ReadStatus()
{
    while (true)
    {
        Console.WriteLine("Status: 1) To read  2) Reading  3) Read");
        Console.Write("Choose: ");
        string input = Console.ReadLine() ?? "";

        if (input == "1") return ReadingStatus.ToRead;
        if (input == "2") return ReadingStatus.Reading;
        if (input == "3") return ReadingStatus.Read;

        Console.WriteLine("Invalid status. Choose 1, 2 or 3.");
    }
}