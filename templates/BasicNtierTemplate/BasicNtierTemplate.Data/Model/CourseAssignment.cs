namespace BasicNtierTemplate.Data.Model
{
    /// <summary>
    /// `CourseInstructor` relationship class. Join table for the 
    /// Instructor-to-Courses many-to-many relationship,
    /// </summary>
    public class CourseAssignment
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public Instructor Instructor { get; set; } = default!;
        public Course Course { get; set; } = default!;
    }
}
