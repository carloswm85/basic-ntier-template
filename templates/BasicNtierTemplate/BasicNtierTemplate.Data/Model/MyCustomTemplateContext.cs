using Microsoft.EntityFrameworkCore;
using BasicNtierTemplate.Data.Entities;

/// <summary>
/// Represents the Entity Framework Core database context for the application.
/// </summary>
public sealed partial class BasicNtierTemplateContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasicNtierTemplateContext"/> class 
    /// using the specified <see cref="DbContextOptions{TContext}"/>.
    /// </summary>
    /// <param name="contextOptions">
    /// The options used to configure the database context, such as the connection string 
    /// and database provider (e.g., SQL Server).
    /// </param>
    public BasicNtierTemplateContext(DbContextOptions<BasicNtierTemplateContext> contextOptions)
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

        // Starts the configuration for the 'Posteo' entity.
        modelBuilder.Entity<Posteo>()
            // Defines that the 'Posteo' entity has *one* related 'Blog' entity.
            // The navigation property from 'Posteo' to 'Blog' is 'p.blog'.
            .HasOne<Blog>(p => p.blog)
            // Defines that the related 'Blog' entity has *many* 'Posteo' entities.
            // The navigation property from 'Blog' to 'Posteo' is 'b.posteos'.
            .WithMany(b => b.posteos)
            // Specifies 'id_blog' in the 'Posteo' entity as the *foreign key*
            // that links 'Posteo' back to the primary key of 'Blog'.
            .HasForeignKey(p => p.blogid);

        // Seed sample Blog data into the database when migrations are applied.
        modelBuilder.Entity<Blog>().HasData(
            new Blog { id = 1, url = "https://example.com/blog1" },
            new Blog { id = 2, url = "https://example.com/blog2" },
            new Blog { id = 3, url = "https://example.com/blog3" },
            new Blog { id = 4, url = "https://example.com/blog4" }
        );

        // Seed sample Post data and associate each post with a corresponding Blog using BlogId.
        modelBuilder.Entity<Posteo>().HasData(
            new Posteo { id = 1, titulo = "Post 1", contenido = "Content for Post 1", blogid = 1 },
            new Posteo { id = 2, titulo = "Post 2", contenido = "Content for Post 2", blogid = 1 },
            new Posteo { id = 3, titulo = "Post A", contenido = "Content for Post A", blogid = 2 },
            new Posteo { id = 4, titulo = "Post B", contenido = "Content for Post B", blogid = 3 },
            new Posteo { id = 5, titulo = "Post C", contenido = "Content for Post C", blogid = 4 },
            new Posteo { id = 6, titulo = "Post D", contenido = "Content for Post D", blogid = 4 }
        );
    }
}

