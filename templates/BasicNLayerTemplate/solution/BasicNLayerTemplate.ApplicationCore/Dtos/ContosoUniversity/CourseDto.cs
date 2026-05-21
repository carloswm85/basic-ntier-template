namespace BasicNLayerTemplate.ApplicationCore.Dtos.ContosoUniversity
{
    public class CourseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Credits { get; set; }

        public ICollection<EnrollmentDto>? Enrollments { get; set; }
    }
}
