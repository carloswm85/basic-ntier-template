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

        #region Contoso University Example

        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Student>().ToTable("Student");

        #endregion
    }
}

