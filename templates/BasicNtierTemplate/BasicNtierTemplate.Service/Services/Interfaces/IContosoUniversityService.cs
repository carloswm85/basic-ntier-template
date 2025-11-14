using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IContosoUniversityService
    {
        Task<Student?> DeleteStudentAsync(int id);
        Task<Student?> GetStudentAsync(int id, bool asNoTracking = false);
        Task<IEnumerable<Student>> GetStudentListAsync();
        Task<PaginatedList<Student>> GetStudentListAsync(
            string currentFilter, int pageIndex, int pageSize,
            string searchString, string sortOrder);
        Task SaveStudentAsync(Student student);
        bool StudentExists(int id);
        Task<Student> UpdateStudentAsync(Student student);
    }
}
