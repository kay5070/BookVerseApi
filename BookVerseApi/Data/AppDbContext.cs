using BookVerseApi.Constants;
using BookVerseApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookVerseApi.Data;

public class AppDbContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .Property(u => u.FirstName).HasMaxLength(256);
        
        modelBuilder.Entity<User>()
            .Property(u => u.LastName).HasMaxLength(256);
        
        // Seed Roles
        modelBuilder.Entity<IdentityRole<Guid>>()
            .HasData(new List<IdentityRole<Guid>>
            {
                new IdentityRole<Guid>()
                {
                    Id = IdentityRoleConstants.AdminRoleGuid,
                    Name = IdentityRoleConstants.Admin,
                    NormalizedName = IdentityRoleConstants.Admin.ToUpper()
                },
                new IdentityRole<Guid>()
                {
                    Id = IdentityRoleConstants.UserRoleGuid,
                    Name = IdentityRoleConstants.User,
                    NormalizedName = IdentityRoleConstants.User.ToUpper()
                }
            });
        
        // Seed Authors
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "George", LastName = "Orwell" },
            new Author { Id = 2, FirstName = "J.K.", LastName = "Rowling" },
            new Author { Id = 3, FirstName = "Jane", LastName = "Austen" },
            new Author { Id = 4, FirstName = "Mark", LastName = "Twain" },
            new Author { Id = 5, FirstName = "Fyodor", LastName = "Dostoevsky" }
        );

        // Seed Books
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "1984",
                Price = 15.99m,
                PublishDate = new DateOnly(1949, 6, 8),
                AuthorId = 1,
                CategoryId = 1 // Dystopian
            },
            new Book
            {
                Id = 2,
                Title = "Harry Potter and the Sorcerer's Stone",
                Price = 19.99m,
                PublishDate = new DateOnly(1997, 6, 26),
                AuthorId = 2,
                CategoryId = 2 // Fantasy
            },
            new Book
            {
                Id = 3,
                Title = "Pride and Prejudice",
                Price = 12.5m,
                PublishDate = new DateOnly(1813, 1, 28),
                AuthorId = 3,
                CategoryId = 3 // Classic
            },
            new Book
            {
                Id = 4,
                Title = "Adventures of Huckleberry Finn",
                Price = 14.2m,
                PublishDate = new DateOnly(1884, 12, 10),
                AuthorId = 4,
                CategoryId = 4 // Adventure
            },
            new Book
            {
                Id = 5,
                Title = "Crime and Punishment",
                Price = 17.75m,
                PublishDate = new DateOnly(1866, 1, 1),
                AuthorId = 5,
                CategoryId = 5 // Philosophical Fiction
            }
        );
        modelBuilder.Entity<Category>().HasData(
            
            new Category { Id = 1, Name = "Dystopian" },
            new Category { Id = 2, Name = "Fantasy" },
            new Category { Id = 3, Name = "Classic" },
            new Category { Id = 4, Name = "Adventure" },
            new Category { Id = 5, Name = "Philosophical Fiction" }
        );
    }
}
    