using ComplexNLayerTemplate.Data.Constants;

namespace ComplexNLayerTemplate.Service.Dtos.ContosoUniversity
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
