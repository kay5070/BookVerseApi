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

    // ✅ REFACTORED: Extracted common logic to eliminate duplication
    private void ApplyAuditInformation()
    {
        var currentUserEmail = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        
        foreach (var entry in entries)
        {
            switch (entry.Entity)
            {
                case Book book:
                    ApplyBookAudit(book, entry.State, currentUserEmail);
                    break;
                case Author author:
                    ApplyAuthorAudit(author, entry.State, currentUserEmail);
                    break;
                case Category category:
                    ApplyCategoryAudit(category, entry.State, currentUserEmail);
                    break;
                case User user:
                    ApplyUserAudit(user, entry.State);
                    break;
                case BookAuthor bookAuthor when entry.State == EntityState.Added:
                    bookAuthor.CreatedAtUtc = DateTime.UtcNow;
                    break;
                case BookCategory bookCategory when entry.State == EntityState.Added:
                    bookCategory.CreatedAtUtc = DateTime.UtcNow;
                    break;
            }
        }
    }

    private void ApplyBookAudit(Book book, EntityState state, string currentUser)
    {
        if (state == EntityState.Added)
        {
            book.CreatedAtUtc = DateTime.UtcNow;
            book.CreatedBy = currentUser;
            book.UpdatedAtUtc = DateTime.UtcNow;
            book.UpdatedBy = currentUser;
        }
        else if (state == EntityState.Modified)
        {
            book.UpdatedAtUtc = DateTime.UtcNow;
            book.UpdatedBy = currentUser;
        }
    }

    private void ApplyAuthorAudit(Author author, EntityState state, string currentUser)
    {
        if (state == EntityState.Added)
        {
            author.CreatedAtUtc = DateTime.UtcNow;
            author.CreatedBy = currentUser;
            author.UpdatedAtUtc = DateTime.UtcNow;
            author.UpdatedBy = currentUser;
        }
        else if (state == EntityState.Modified)
        {
            author.UpdatedAtUtc = DateTime.UtcNow;
            author.UpdatedBy = currentUser;
        }
    }

    private void ApplyCategoryAudit(Category category, EntityState state, string currentUser)
    {
        if (state == EntityState.Added)
        {
            category.CreatedAtUtc = DateTime.UtcNow;
            category.CreatedBy = currentUser;
            category.UpdatedAtUtc = DateTime.UtcNow;
            category.UpdatedBy = currentUser;
        }
        else if (state == EntityState.Modified)
        {
            category.UpdatedAtUtc = DateTime.UtcNow;
            category.UpdatedBy = currentUser;
        }
    }

    private void ApplyUserAudit(User user, EntityState state)
    {
        if (state == EntityState.Added)
        {
            user.CreatedAtUtc = DateTime.UtcNow;
            user.UpdatedAtUtc = DateTime.UtcNow;
        }
        else if (state == EntityState.Modified)
        {
            user.UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditInformation();
        return base.SaveChanges();
    }
}