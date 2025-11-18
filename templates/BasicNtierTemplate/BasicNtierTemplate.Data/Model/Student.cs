namespace BasicNtierTemplate.Data.Model
{
    public partial class Student
    {
        public int Id { get; set; }
        public required string GovernmentId { get; set; }
        public required string LastName { get; set; }
        public required string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
