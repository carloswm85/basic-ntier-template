using System.ComponentModel.DataAnnotations;
using BasicNtierTemplate.Data.Enums;

namespace BasicNtierTemplate.Data.Model
{
    /// <summary>
    /// There's a many-to-many relationship between the Student and Course 
    /// entities,and the Enrollment entity functions as a many-to-many join
    /// table with payload in the database. "With payload" means that the 
    /// Enrollment table contains additional data besides foreign keys for 
    /// the joined tables (in this case, a primary key and a Grade property).
    /// </summary>
    public partial class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }
}
