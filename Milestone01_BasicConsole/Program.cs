// Milestone 01 - Basic Console Application

using System.Text.Json;

const string FilePath = "books.json";
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

List<Dictionary<string, string>> books = LoadBooks();
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
    Console.WriteLine();
    Console.WriteLine("==== Add book ====");

    string title = ReadUniqueTitle();
    string author = ReadNonEmpty("Author: ");
    int year = ReadYear("Year: ");
    string status = ReadStatus();

    var book = new Dictionary<string, string>
    {
        ["Id"] = GetNextId().ToString(),
        ["Title"] = title,
        ["Author"] = author,
        ["Year"] = year.ToString(),
        ["Status"] = status
    };

    books.Add(book);
    SaveBooks();
    Console.WriteLine("Book added successfully.");
}

void ListBooks()
{
    Console.WriteLine();
    Console.WriteLine("==== All books ====");

    if (books.Count == 0)
    {
        Console.WriteLine("There are no books yet.");
        return;
    }

    foreach (var book in books)
    {
        PrintBook(book);
    }
}

void FindBooks()
{
    Console.WriteLine();
    Console.WriteLine("==== Find book ====");
    string text = ReadNonEmpty("Search by title or author: ").ToLower();

    bool found = false;
    foreach (var book in books)
    {
        if (book["Title"].ToLower().Contains(text) || book["Author"].ToLower().Contains(text))
        {
            PrintBook(book);
            found = true;
        }
    }

    if (!found)
    {
        Console.WriteLine("No books found.");
    }
}

void DeleteBook()
{
    Console.WriteLine();
    Console.WriteLine("==== Delete book ====");

    if (books.Count == 0)
    {
        Console.WriteLine("There are no books to delete.");
        return;
    }

    Console.Write("Enter the Id of the book to delete: ");
    string id = Console.ReadLine() ?? "";

    Dictionary<string, string>? bookToDelete = null;
    foreach (var book in books)
    {
        if (book["Id"] == id)
        {
            bookToDelete = book;
            break;
        }
    }

    if (bookToDelete == null)
    {
        Console.WriteLine("No book with that Id.");
        return;
    }

    books.Remove(bookToDelete);
    SaveBooks();
    Console.WriteLine("Book deleted.");
}

// Aux methods

void PrintBook(Dictionary<string, string> book)
{
    Console.WriteLine($"[{book["Id"]}] {book["Title"]} - {book["Author"]} ({book["Year"]}) | {book["Status"]}");
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

bool TitleExists(string title)
{
    foreach (var book in books)
    {
        if (book["Title"].ToLower() == title.ToLower())
        {
            return true;
        }
    }
    return false;
}

string ReadUniqueTitle()
{
    while (true)
    {
        string title = ReadNonEmpty("Title: ");

        if (!TitleExists(title))
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

string ReadStatus()
{
    while (true)
    {
        Console.WriteLine("Status: 1) To read  2) Reading  3) Read");
        Console.Write("Choose: ");
        string input = Console.ReadLine() ?? "";

        if (input == "1") return "To read";
        if (input == "2") return "Reading";
        if (input == "3") return "Read";

        Console.WriteLine("Invalid status. Choose 1, 2 or 3.");
    }
}

int GetNextId()
{
    int maxId = 0;
    foreach (var book in books)
    {
        int id = int.Parse(book["Id"]);
        if (id > maxId)
        {
            maxId = id;
        }
    }
    return maxId + 1;
}

// JSON persistence

List<Dictionary<string, string>> LoadBooks()
{
    if (!File.Exists(FilePath))
    {
        return new List<Dictionary<string, string>>();
    }

    string json = File.ReadAllText(FilePath);
    var loaded = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
    return loaded ?? new List<Dictionary<string, string>>();
}

void SaveBooks()
{
    string json = JsonSerializer.Serialize(books, jsonOptions);
    File.WriteAllText(FilePath, json);
}