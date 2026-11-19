# Project Evolution Portfolio - Personal Library

A personal library app to keep track of books (to read, reading, read).
The same project evolves through five milestones.


## Milestone 01 - Basic Console Application

A console menu to add, list, find and delete books, saved in a JSON file (books.json).

Concepts used: variables, conditions, loops, methods, collections and JSON persistence.
Each book is a dictionary (Id, Title, Author, Year, Status), and titles must be unique.

How to run: open a terminal in Milestone01_BasicConsole and run dotnet run.



## Milestone 02 - OOP and Persistence

Same app, now modelled with classes: Book (properties, constructor and encapsulation with validation) and Library (the collection of books and the JSON persistence). Program.cs only has the console menu.

What changed: the dictionaries were replaced by a Book class, so the data is typed and validated. The status is now an enum.

How to run: open a terminal in Milestone02_OOP and run dotnet run.



## Milestone 03 - Layered Multi-user Console Application

The app is split into layers: Presentation (ConsoleUI), Services (AuthService, BookService) and Repositories (UserRepository, BookRepository, JSON files). Users can register, log in and log out, and each user only sees their own books.

What changed: the code moved from one Library class to separate layers. Validation rules are now in the service layer, and every book has an OwnerId.

How to run: open a terminal in Milestone03_LayeredConsole and run dotnet run.



## Milestone 04 - Database Application

The JSON files were replaced by a relational SQLite database (library.db) accessed with Dapper. Tables: Users, Genres and Books. Books has a foreign key to Users (each book belongs to a user) and another to Genres (each book has a genre). The SQL schema is in Data/schema.sql and example queries in Data/queries.sql.

What changed: the repositories now run SQL (INSERT, SELECT with JOIN, LIKE, COUNT with GROUP BY, DELETE) instead of reading and writing JSON. Services and presentation keep the same layers. New: genres and a statistics option.

Database concepts: primary keys, foreign keys, one-to-many relationships, UNIQUE constraints (username, and title per user), CHECK constraint, JOIN and GROUP BY.

How to run: open a terminal in Milestone04_Database, run dotnet add package Dapper and dotnet add package Microsoft.Data.Sqlite if the packages are missing, then dotnet run.



## Milestone 05 - Basic Web API

The library is exposed as an ASP.NET Core Web API (Milestone05_WebAPI) that reuses the SQLite database and Dapper repositories from Milestone 04. Endpoints for Books (GET, POST, PUT, DELETE), plus Genres (GET) and Users (POST, GET) so that books can reference existing users and genres. Swagger UI is available at /swagger.

What changed: the console presentation layer was replaced by controllers. Requests and responses use DTOs, input is validated with data annotations and service rules, and errors return the correct HTTP status codes (400, 404, 409).

Backend concepts added: REST controllers, DTOs, model validation, HTTP status codes, dependency injection and Swagger/OpenAPI.

How to run: open a terminal in Milestone05_WebAPI, run dotnet run and open http://localhost:5080/swagger.