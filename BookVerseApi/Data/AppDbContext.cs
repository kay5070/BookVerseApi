using BookVerseApi.Constants;
using BookVerseApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookVerseApi.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) :
        base(options)
    {
        _httpContextAccessor = httpContextAccessor;
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
                new()
                {
                    Id = IdentityRoleConstants.AdminRoleGuid,
                    Name = IdentityRoleConstants.Admin,
                    NormalizedName = IdentityRoleConstants.Admin.ToUpper()
                },
                new()
                {
                    Id = IdentityRoleConstants.UserRoleGuid,
                    Name = IdentityRoleConstants.User,
                    NormalizedName = IdentityRoleConstants.User.ToUpper()
                }
            });

        // Seed Authors
        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 1, FirstName = "George", LastName = "Orwell", CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Author
            {
                Id = 2, FirstName = "J.K.", LastName = "Rowling", CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Author
            {
                Id = 3, FirstName = "Jane", LastName = "Austen", CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Author
            {
                Id = 4, FirstName = "Mark", LastName = "Twain", CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Author
            {
                Id = 5, FirstName = "Fyodor", LastName = "Dostoevsky", CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            }
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
                ,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Book
            {
                Id = 2,
                Title = "Harry Potter and the Sorcerer's Stone",
                Price = 19.99m,
                PublishDate = new DateOnly(1997, 6, 26),
                AuthorId = 2,
                CategoryId = 2 // Fantasy
                ,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Book
            {
                Id = 3,
                Title = "Pride and Prejudice",
                Price = 12.5m,
                PublishDate = new DateOnly(1813, 1, 28),
                AuthorId = 3,
                CategoryId = 3 // Classic
                ,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Book
            {
                Id = 4,
                Title = "Adventures of Huckleberry Finn",
                Price = 14.2m,
                PublishDate = new DateOnly(1884, 12, 10),
                AuthorId = 4,
                CategoryId = 4 // Adventure
                ,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Book
            {
                Id = 5,
                Title = "Crime and Punishment",
                Price = 17.75m,
                PublishDate = new DateOnly(1866, 1, 1),
                AuthorId = 5,
                CategoryId = 5 // Philosophical Fiction
                ,
                CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow, CreatedBy = "System",
                UpdatedBy = "System"
            }
        );
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1, Name = "Dystopian", CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Category
            {
                Id = 2, Name = "Fantasy", CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Category
            {
                Id = 3, Name = "Classic", CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Category
            {
                Id = 4, Name = "Adventure", CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Category
            {
                Id = 5, Name = "Philosophical Fiction", CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System"
            }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserEmail = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        foreach (var entry in entries)
        {
            if (entry.Entity is Book book)
            {
                if (entry.State == EntityState.Added)
                {
                    book.CreatedAtUtc = DateTime.UtcNow;
                    book.CreatedBy = currentUserEmail;
                    book.UpdatedAtUtc = DateTime.UtcNow;
                    book.UpdatedBy = currentUserEmail;
                }
                else if (entry.State == EntityState.Modified)
                {
                    book.UpdatedAtUtc = DateTime.UtcNow;
                    book.UpdatedBy = currentUserEmail;
                }
            }

            if (entry.Entity is Author author)
            {
                if (entry.State == EntityState.Added)
                {
                    author.CreatedAtUtc = DateTime.UtcNow;
                    author.CreatedBy = currentUserEmail;
                    author.UpdatedAtUtc = DateTime.UtcNow;
                    author.UpdatedBy = currentUserEmail;
                }
                else if (entry.State == EntityState.Modified)
                {
                    author.UpdatedAtUtc = DateTime.UtcNow;
                    author.UpdatedBy = currentUserEmail;
                }
            }

            if (entry.Entity is Category category)
            {
                if (entry.State == EntityState.Added)
                {
                    category.CreatedAtUtc = DateTime.UtcNow;
                    category.CreatedBy = currentUserEmail;
                    category.UpdatedAtUtc = DateTime.UtcNow;
                    category.UpdatedBy = currentUserEmail;
                }
                else if (entry.State == EntityState.Modified)
                {
                    category.UpdatedAtUtc = DateTime.UtcNow;
                    category.UpdatedBy = currentUserEmail;
                }
            }

            if (entry.Entity is User user)
            {
                if (entry.State == EntityState.Added)
                {
                    user.CreatedAtUtc = DateTime.UtcNow;
                    user.UpdatedAtUtc = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    user.UpdatedAtUtc = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var currentUserEmail = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        foreach (var entry in entries)
        {
            if (entry.Entity is Book book)
            {
                if (entry.State == EntityState.Added)
                {
                    book.CreatedAtUtc = DateTime.UtcNow;
                    book.CreatedBy = currentUserEmail;
                    book.UpdatedAtUtc = DateTime.UtcNow;
                    book.UpdatedBy = currentUserEmail;
                }
                else if (entry.State == EntityState.Modified)
                {
                    book.UpdatedAtUtc = DateTime.UtcNow;
                    book.UpdatedBy = currentUserEmail;
                }
            }

            if (entry.Entity is Author author)
            {
                if (entry.State == EntityState.Added)
                {
                    author.CreatedAtUtc = DateTime.UtcNow;
                    author.CreatedBy = currentUserEmail;
                    author.UpdatedAtUtc = DateTime.UtcNow;
                    author.UpdatedBy = currentUserEmail;
                }
                else if (entry.State == EntityState.Modified)
                {
                    author.UpdatedAtUtc = DateTime.UtcNow;
                    author.UpdatedBy = currentUserEmail;
                }
            }

            if (entry.Entity is Category category)
            {
                if (entry.State == EntityState.Added)
                {
                    category.CreatedAtUtc = DateTime.UtcNow;
                    category.CreatedBy = currentUserEmail;
                    category.UpdatedAtUtc = DateTime.UtcNow;
                    category.UpdatedBy = currentUserEmail;
                }
                else if (entry.State == EntityState.Modified)
                {
                    category.UpdatedAtUtc = DateTime.UtcNow;
                    category.UpdatedBy = currentUserEmail;
                }
            }

            if (entry.Entity is User user)
            {
                if (entry.State == EntityState.Added)
                {
                    user.CreatedAtUtc = DateTime.UtcNow;
                    user.UpdatedAtUtc = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    user.UpdatedAtUtc = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChanges();
    }
}