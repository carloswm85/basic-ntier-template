using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicNtierTemplate.Data.Model
{
    public partial class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public int? InstructorId { get; set; }

        public Instructor? Administrator { get; set; }
        public ICollection<Course> Courses { get; set; } = [];
    }
}

/* By convention, the Entity Framework enables cascade delete for non-nullable
 * foreign keys and for many-to-many relationships. This can result in circular
 * cascade delete rules, which will cause an exception when you try to add a
 * migration.
 * 
 * For example, if you didn't define the Department.InstructorID property as
 * nullable, EF would configure a cascade delete rule to delete the department
 * when you delete the instructor, which isn't what you want to have happen.
 * If your business rules required the InstructorID property to be non-nullable,
 * you would have to use the following fluent API statement to disable cascade
 * delete on the relationship:
 */

// C#
// modelBuilder.Entity<Department>()
//    .HasOne(d => d.Administrator)
//    .WithMany()
//    .OnDelete(DeleteBehavior.Restrict)