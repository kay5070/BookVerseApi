using BookVerse.Core.Constants;
using BookVerse.Core.Entities;
using BookVerse.Infrastructure.Data.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Infrastructure.Data;

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
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User properties
        modelBuilder.Entity<User>()
            .Property(u => u.FirstName).HasMaxLength(256);
        modelBuilder.Entity<User>()
            .Property(u => u.LastName).HasMaxLength(256);

        // Configure Many-to-Many: Book-Author
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });
        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(ba => ba.BookAuthors)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Many-to-Many: Book-Category
        modelBuilder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        SeedData(modelBuilder);
    }

    public void SeedData(ModelBuilder modelBuilder)
    {
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
        
        //Seed Authors, Categories, Books, and Relationships
        modelBuilder.Entity<Author>().HasData(AuthorSeed.GetAuthors());
        modelBuilder.Entity<Category>().HasData(CategorySeed.GetCategories());
        modelBuilder.Entity<Book>().HasData(BookSeed.GetBooks());
        modelBuilder.Entity<BookAuthor>().HasData(BookAuthorSeed.GetBookAuthors());
        modelBuilder.Entity<BookCategory>().HasData(BookCategorySeed.GetBookCategories());
        
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

            else if (entry.Entity is Author author)
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

            else if (entry.Entity is Category category)
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

            else if (entry.Entity is User user)
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

            else if (entry.Entity is BookAuthor bookAuthor && entry.State == EntityState.Added)
            {
                bookAuthor.CreatedAtUtc = DateTime.UtcNow;
            }
            else if (entry.Entity is BookCategory bookCategory && entry.State == EntityState.Added)
            {
                bookCategory.CreatedAtUtc = DateTime.UtcNow;
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