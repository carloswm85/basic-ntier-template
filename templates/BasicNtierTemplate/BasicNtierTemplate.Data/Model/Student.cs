using System.ComponentModel.DataAnnotations.Schema;

namespace BasicNtierTemplate.Data.Model
{
    public partial class Student
    {
        public int Id { get; set; }
        public string GovernmentId { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Column("FirstName")]
        public string FirstMidName { get; set; } = string.Empty;

        public DateOnly EnrollmentDate { get; set; }

        [NotMapped]  // Explicitly mark as not mapped to DB
        public string FullName => $"{LastName}, {FirstMidName}";

        public List<Enrollment> Enrollments { get; set; } = [];
    }
}
