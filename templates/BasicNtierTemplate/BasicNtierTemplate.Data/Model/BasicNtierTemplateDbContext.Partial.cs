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
        var blogId1 = Guid.Parse("cbb7a4b3-4c52-4c4d-9b73-6e2c03d6e42a");
        var blogId2 = Guid.Parse("e3e2f07a-b2a1-46b5-bd1f-705a2e38e4da");
        var blogId3 = Guid.Parse("2fa22936-6767-4bb8-94c3-f09d78d8b1e3");
        var blogId4 = Guid.Parse("61bb6ee4-d9fc-4de1-8a9a-20a2df0cf9b4");

        modelBuilder.Entity<Blog>().HasData(
            new Blog { id = blogId1, url = "https://example.com/blog1" },
            new Blog { id = blogId2, url = "https://example.com/blog2" },
            new Blog { id = blogId3, url = "https://example.com/blog3" },
            new Blog { id = blogId4, url = "https://example.com/blog4" }
        );

        // Seed sample Post data and associate each post with a corresponding Blog using BlogId.
        modelBuilder.Entity<Posteo>().HasData(
            new Posteo { id = Guid.Parse("e17b4b9a-3a0b-4a68-b2f9-2a2cc6cba3b3"), titulo = "Post 1", contenido = "Content for Post 1", blogid = blogId1 },
            new Posteo { id = Guid.Parse("c621b541-34c9-44c5-84b2-58d9e7b02c22"), titulo = "Post 2", contenido = "Content for Post 2", blogid = blogId1 },
            new Posteo { id = Guid.Parse("7fd7b1ef-2c6a-4f12-8e61-0d1b5f1db15a"), titulo = "Post A", contenido = "Content for Post A", blogid = blogId2 },
            new Posteo { id = Guid.Parse("d95bbcf6-bf1a-4b77-9f24-24d6574acac4"), titulo = "Post B", contenido = "Content for Post B", blogid = blogId3 },
            new Posteo { id = Guid.Parse("a40844b7-cd8f-4a53-bbc6-198f3e4a95f7"), titulo = "Post C", contenido = "Content for Post C", blogid = blogId4 },
            new Posteo { id = Guid.Parse("f28c72cb-9b92-4d2e-9a4e-6e9a4177c2f5"), titulo = "Post D", contenido = "Content for Post D", blogid = blogId4 }
        );

        #endregion

        #region Contoso University Example

        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Student>().ToTable("Student");

        #endregion
    }
}

