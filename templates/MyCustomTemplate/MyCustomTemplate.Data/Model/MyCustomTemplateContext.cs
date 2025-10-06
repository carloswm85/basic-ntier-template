using Microsoft.EntityFrameworkCore;
using MyCustomTemplate.Data.Entities;

/// <summary>
/// Represents the Entity Framework Core database context for the application.
/// </summary>
public sealed partial class MyCustomTemplateContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyCustomTemplateContext"/> class 
    /// using the specified <see cref="DbContextOptions{TContext}"/>.
    /// </summary>
    /// <param name="contextOptions">
    /// The options used to configure the database context, such as the connection string 
    /// and database provider (e.g., SQL Server).
    /// </param>
    public MyCustomTemplateContext(DbContextOptions<MyCustomTemplateContext> contextOptions)
        : base(contextOptions)
    {
        // The `contextOptions` parameter passes connection details and provider configuration
        // to the base DbContext constructor.
    }

    /// <summary>
    /// Configures the entity model and seeds initial data for the database.
    /// </summary>
    /// <param name="modelBuilder">
    /// Provides a simple API to configure entity relationships, constraints, and seed data.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Always call the base method first to ensure EF Core's default configurations are applied.
        base.OnModelCreating(modelBuilder);

        // Seed sample Blog data into the database when migrations are applied.
        modelBuilder.Entity<Blog>().HasData(
            new Blog { BlogId = 1, Url = "https://example.com/blog1" },
            new Blog { BlogId = 2, Url = "https://example.com/blog2" },
            new Blog { BlogId = 3, Url = "https://example.com/blog3" },
            new Blog { BlogId = 4, Url = "https://example.com/blog4" }
        );

        // Seed sample Post data and associate each post with a corresponding Blog using BlogId.
        modelBuilder.Entity<Post>().HasData(
            new Post { PostId = 1, Title = "Post 1", Content = "Content for Post 1", BlogId = 1 },
            new Post { PostId = 2, Title = "Post 2", Content = "Content for Post 2", BlogId = 1 },
            new Post { PostId = 3, Title = "Post A", Content = "Content for Post A", BlogId = 2 },
            new Post { PostId = 4, Title = "Post B", Content = "Content for Post B", BlogId = 3 },
            new Post { PostId = 5, Title = "Post C", Content = "Content for Post C", BlogId = 4 },
            new Post { PostId = 6, Title = "Post D", Content = "Content for Post D", BlogId = 4 }
        );
    }
}

