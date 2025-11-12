using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IContosoUniversityService
    {
        Task DeleteStudent(int id);
        Task<Student?> GetStudentAsync(int id);
        Task<List<Student>> GetStudentListAsync();
        Task SaveStudentAsync(Student student);
        bool StudentExists(int id);
        Task<Student> UpdateStudent(Student student);
    }
}
