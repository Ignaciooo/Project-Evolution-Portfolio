SELECT Id, Username, PasswordHash FROM Users WHERE Username = 'ana';

SELECT b.Id, b.Title, b.Author, b.Year, b.Status, g.Name AS Genre
FROM Books b
JOIN Genres g ON g.Id = b.GenreId
WHERE b.UserId = 1
ORDER BY b.Title;

SELECT b.Id, b.Title, b.Author
FROM Books b
WHERE b.UserId = 1 AND (b.Title LIKE '%dune%' OR b.Author LIKE '%dune%');

SELECT g.Name AS Genre, COUNT(b.Id) AS Total
FROM Books b
JOIN Genres g ON g.Id = b.GenreId
WHERE b.UserId = 1
GROUP BY g.Name
ORDER BY Total DESC;

SELECT u.Username, COUNT(b.Id) AS Total
FROM Users u
LEFT JOIN Books b ON b.UserId = u.Id
GROUP BY u.Username;

DELETE FROM Books WHERE Id = 1 AND UserId = 1;