using BasicNtierTemplate.Data.Enums;

namespace BasicNtierTemplate.Data.Model
{
    public partial class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Grade? Grade { get; set; }

        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }
}
