using BasicNtierTemplate.Data.Enums;

namespace BasicNtierTemplate.Service.Dtos
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? StudentId { get; set; }
        public Grade? Grade { get; set; }
        public CourseDto? Course { get; set; }
        public StudentDto? Student { get; set; }
    }
}
