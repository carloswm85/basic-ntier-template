namespace BasicNtierTemplate.Data.Model
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public partial class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
