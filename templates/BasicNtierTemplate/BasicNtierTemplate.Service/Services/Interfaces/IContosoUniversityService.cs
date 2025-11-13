using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IContosoUniversityService
    {
        Task<Student?> DeleteStudentAsync(int id);
        Task<Student?> GetStudentAsync(int id, bool asNoTracking = false);
        Task<List<Student>> GetStudentListAsync();
        Task SaveStudentAsync(Student student);
        bool StudentExists(int id);
        Task<Student> UpdateStudentAsync(Student student);
    }
}
