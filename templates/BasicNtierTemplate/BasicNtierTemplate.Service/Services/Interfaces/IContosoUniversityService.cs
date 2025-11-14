using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IContosoUniversityService
    {
        Task<Student?> DeleteStudentAsync(int id);
        Task<Student?> GetStudentAsync(int id, bool asNoTracking = false);
        Task<IEnumerable<Student>> GetStudentListAsync();
        Task<IEnumerable<Student>> GetStudentListAsync(
            string sortOrder, string currentFilter,
            string searchString, int? pageNumber);
        Task SaveStudentAsync(Student student);
        bool StudentExists(int id);
        Task<Student> UpdateStudentAsync(Student student);
    }
}
