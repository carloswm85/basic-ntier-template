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
        var blogId1 = Guid.NewGuid();
        var blogId2 = Guid.NewGuid();
        var blogId3 = Guid.NewGuid();
        var blogId4 = Guid.NewGuid();

        modelBuilder.Entity<Blog>().HasData(
            new Blog { id = blogId1, url = "https://example.com/blog1" },
            new Blog { id = blogId2, url = "https://example.com/blog2" },
            new Blog { id = blogId3, url = "https://example.com/blog3" },
            new Blog { id = blogId4, url = "https://example.com/blog4" }
        );

        // Seed sample Post data and associate each post with a corresponding Blog using BlogId.
        modelBuilder.Entity<Posteo>().HasData(
            new Posteo { id = Guid.NewGuid(), titulo = "Post 1", contenido = "Content for Post 1", blogid = blogId1 },
            new Posteo { id = Guid.NewGuid(), titulo = "Post 2", contenido = "Content for Post 2", blogid = blogId1 },
            new Posteo { id = Guid.NewGuid(), titulo = "Post A", contenido = "Content for Post A", blogid = blogId2 },
            new Posteo { id = Guid.NewGuid(), titulo = "Post B", contenido = "Content for Post B", blogid = blogId3 },
            new Posteo { id = Guid.NewGuid(), titulo = "Post C", contenido = "Content for Post C", blogid = blogId4 },
            new Posteo { id = Guid.NewGuid(), titulo = "Post D", contenido = "Content for Post D", blogid = blogId4 }
        );

        #endregion

        #region Contoso University Example

        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Student>().ToTable("Student");

        #endregion
    }
}

