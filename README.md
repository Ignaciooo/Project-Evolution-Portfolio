# Project Evolution Portfolio - Personal Library

A personal library app to keep track of books (to read, reading, read).
The same project evolves through five milestones.

## Milestone 01 - Basic Console Application
A console menu to add, list, find and delete books, saved in a JSON file (`books.json`).

Concepts used: variables, conditions, loops, methods, collections and JSON persistence.
Each book is a dictionary (`Id`, `Title`, `Author`, `Year`, `Status`), and titles must be unique.

How to run: open a terminal in `Milestone01_BasicConsole` and run `dotnet run`.

## Milestone 02 - OOP and Persistence
Same app, now modelled with classes: `Book` (properties, constructor and encapsulation with validation) and `Library` (the collection of books and the JSON persistence). `Program.cs` only has the console menu.

What changed: the dictionaries were replaced by a `Book` class, so the data is typed and validated. The status is now an enum.

How to run: open a terminal in `Milestone02_OOP` and run `dotnet run`.