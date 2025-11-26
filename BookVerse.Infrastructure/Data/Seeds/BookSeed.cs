using BookVerse.Core.Entities;

namespace BookVerse.Infrastructure.Data.Seeds;

public static class BookSeed
{
    public static List<Book> GetBooks()
    {
        var now = DateTime.UtcNow;
        return new List<Book>
        {
            // Classic Literature
            new()
            {
                Id = 1, Title = "1984", Price = 15.99m, PublishDate = new DateOnly(1949, 6, 8), CreatedAtUtc = now,
                UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 2, Title = "Animal Farm", Price = 12.99m, PublishDate = new DateOnly(1945, 8, 17),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 3, Title = "Pride and Prejudice", Price = 11.99m, PublishDate = new DateOnly(1813, 1, 28),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 4, Title = "Adventures of Huckleberry Finn", Price = 13.50m,
                PublishDate = new DateOnly(1884, 12, 10), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 5, Title = "Crime and Punishment", Price = 16.75m, PublishDate = new DateOnly(1866, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 6, Title = "The Brothers Karamazov", Price = 18.99m, PublishDate = new DateOnly(1880, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 7, Title = "War and Peace", Price = 22.50m, PublishDate = new DateOnly(1869, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 8, Title = "Anna Karenina", Price = 17.99m, PublishDate = new DateOnly(1878, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 9, Title = "Great Expectations", Price = 14.25m, PublishDate = new DateOnly(1861, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 10, Title = "A Tale of Two Cities", Price = 13.99m, PublishDate = new DateOnly(1859, 4, 30),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // American Classics
            new()
            {
                Id = 11, Title = "The Great Gatsby", Price = 14.99m, PublishDate = new DateOnly(1925, 4, 10),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 12, Title = "Moby-Dick", Price = 16.50m, PublishDate = new DateOnly(1851, 10, 18),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 13, Title = "Wuthering Heights", Price = 12.75m, PublishDate = new DateOnly(1847, 12, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 14, Title = "Jane Eyre", Price = 13.50m, PublishDate = new DateOnly(1847, 10, 16),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // Fantasy
            new()
            {
                Id = 15, Title = "The Lord of the Rings", Price = 29.99m, PublishDate = new DateOnly(1954, 7, 29),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 16, Title = "The Hobbit", Price = 18.99m, PublishDate = new DateOnly(1937, 9, 21),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 17, Title = "Harry Potter and the Philosopher's Stone", Price = 19.99m,
                PublishDate = new DateOnly(1997, 6, 26), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 18, Title = "Harry Potter and the Chamber of Secrets", Price = 19.99m,
                PublishDate = new DateOnly(1998, 7, 2), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 19, Title = "The Chronicles of Narnia", Price = 24.99m, PublishDate = new DateOnly(1950, 10, 16),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // Science Fiction
            new()
            {
                Id = 20, Title = "Foundation", Price = 15.99m, PublishDate = new DateOnly(1951, 6, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 21, Title = "I, Robot", Price = 14.50m, PublishDate = new DateOnly(1950, 12, 2),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 22, Title = "Dune", Price = 21.99m, PublishDate = new DateOnly(1965, 8, 1), CreatedAtUtc = now,
                UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 23, Title = "The Hitchhiker's Guide to the Galaxy", Price = 16.99m,
                PublishDate = new DateOnly(1979, 10, 12), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 24, Title = "Fahrenheit 451", Price = 14.99m, PublishDate = new DateOnly(1953, 10, 19),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 25, Title = "Brave New World", Price = 15.50m, PublishDate = new DateOnly(1932, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 26, Title = "Do Androids Dream of Electric Sheep?", Price = 14.99m,
                PublishDate = new DateOnly(1968, 1, 1), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 27, Title = "The Left Hand of Darkness", Price = 15.99m, PublishDate = new DateOnly(1969, 3, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // Modern Fiction
            new()
            {
                Id = 28, Title = "One Hundred Years of Solitude", Price = 17.99m,
                PublishDate = new DateOnly(1967, 5, 30), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 29, Title = "Norwegian Wood", Price = 16.50m, PublishDate = new DateOnly(1987, 9, 4),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 30, Title = "Kafka on the Shore", Price = 17.25m, PublishDate = new DateOnly(2002, 9, 12),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 31, Title = "Beloved", Price = 15.99m, PublishDate = new DateOnly(1987, 9, 1), CreatedAtUtc = now,
                UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 32, Title = "The Handmaid's Tale", Price = 16.99m, PublishDate = new DateOnly(1985, 8, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 33, Title = "The Road", Price = 15.50m, PublishDate = new DateOnly(2006, 9, 26),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 34, Title = "Slaughterhouse-Five", Price = 14.99m, PublishDate = new DateOnly(1969, 3, 31),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 35, Title = "The Shining", Price = 18.99m, PublishDate = new DateOnly(1977, 1, 28),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // Mystery & Thriller
            new()
            {
                Id = 36, Title = "Murder on the Orient Express", Price = 14.50m, PublishDate = new DateOnly(1934, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 37, Title = "And Then There Were None", Price = 13.99m, PublishDate = new DateOnly(1939, 11, 6),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 38, Title = "The Hound of the Baskervilles", Price = 12.99m,
                PublishDate = new DateOnly(1902, 4, 1), CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new()
            {
                Id = 39, Title = "The Da Vinci Code", Price = 16.99m, PublishDate = new DateOnly(2003, 3, 18),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // Philosophy & Non-Fiction
            new()
            {
                Id = 40, Title = "The Stranger", Price = 13.50m, PublishDate = new DateOnly(1942, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 41, Title = "Man's Search for Meaning", Price = 14.99m, PublishDate = new DateOnly(1946, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 42, Title = "The Art of War", Price = 11.99m, PublishDate =new DateOnly(1910, 1, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },

            // Additional Classics
            new()
            {
                Id = 43, Title = "The Old Man and the Sea", Price = 12.99m, PublishDate = new DateOnly(1952, 9, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 44, Title = "For Whom the Bell Tolls", Price = 15.99m, PublishDate = new DateOnly(1940, 10, 21),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 45, Title = "Mrs Dalloway", Price = 13.50m, PublishDate = new DateOnly(1925, 5, 14),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 46, Title = "The Picture of Dorian Gray", Price = 12.99m, PublishDate = new DateOnly(1890, 7, 1),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 47, Title = "Lord of the Flies", Price = 13.99m, PublishDate = new DateOnly(1954, 9, 17),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 48, Title = "Of Mice and Men", Price = 11.99m, PublishDate = new DateOnly(1937, 2, 25),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 49, Title = "To Kill a Mockingbird", Price = 14.99m, PublishDate = new DateOnly(1960, 7, 11),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
            new()
            {
                Id = 50, Title = "The Catcher in the Rye", Price = 13.99m, PublishDate = new DateOnly(1951, 7, 16),
                CreatedAtUtc = now, UpdatedAtUtc = now, CreatedBy = "System", UpdatedBy = "System"
            },
        };
    }
}