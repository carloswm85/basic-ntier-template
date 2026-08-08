using CleanArchitectureTemplate.ApplicationCore.Entities;
using CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;
using CleanArchitectureTemplate.Infrastructure.Model.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Model;

/// <summary>
/// Partial class for CleanArchitectureTemplateContext.
/// </summary>
public partial class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    #region Contoso University Example

    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
    public DbSet<CourseAssignment> CourseAssignments { get; set; }

    #endregion

    #region Identity

    public DbSet<Contact> Contact { get; set; }

    #endregion

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

        #region Identity

        modelBuilder.Entity<Contact>().ToTable("Contact");

        #endregion


        #region Contoso University

        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<Department>().ToTable("Department");
        modelBuilder.Entity<Instructor>().ToTable("Instructor");
        modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
        modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");

        modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseId, c.InstructorId });

        #endregion
    }
}
