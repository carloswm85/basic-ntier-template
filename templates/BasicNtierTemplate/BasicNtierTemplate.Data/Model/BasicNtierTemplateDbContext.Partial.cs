using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Data.Model;

/// <summary>
/// Partial class for BasicNtierTemplateContext.
/// </summary>
public partial class BasicNtierTemplateDbContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }

    /// <summary>
    /// Example content for the partial class to configure the model.
    /// 
    /// This `OnModelCreatingPartial` mathod is used for:
    /// - Keeps your custom configurations separate from auto-generated code.
    /// - Prevents losing changes if you re-scaffold the database.
    /// - Follows the partial class pattern (a clean extension mechanism).
    /// </summary>
    /// <param name="modelBuilder"></param>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Always call the base method first to ensure EF Core's default configurations are applied.
        base.OnModelCreating(modelBuilder);

        #region Blog Example

        // Blog DATA IS SEEDED when running the MVC Application or when applying migrations.
        // Starts the configuration for the 'Posteo' entity.
        modelBuilder.Entity<Posteo>()
            .HasOne<Blog>(p => p.blog)
            .WithMany(b => b.Posteos)
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

        #endregion

        #region Contoso University Example

        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Student>().ToTable("Student");

        #endregion
    }
}

