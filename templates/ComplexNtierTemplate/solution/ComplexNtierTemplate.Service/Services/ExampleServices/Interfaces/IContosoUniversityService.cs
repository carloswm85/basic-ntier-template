using ComplexNtierTemplate.Service.Dtos.ContosoUniversity;
using ComplexNtierTemplate.Service.Models;

namespace ComplexNtierTemplate.Service.Services.ExampleServices.Interfaces
{
    public interface IContosoUniversityService
    {
        #region Student

        Task<bool> DeleteStudentAsync(int studentId);
        Task<StudentDto?> GetStudentAsync(int studentId, bool asNoTracking = false);
        Task<IEnumerable<StudentDto>> GetStudentListAsync();
        Task<PaginatedList<StudentDto>> GetStudentsPaginatedListAsync(
            string currentFilter, int pageIndex, int pageSize,
            string searchString, string sortOrder);
        Task<int> CreateStudentAsync(StudentDto studentDto);
        bool StudentExists(int studentId);
        bool StudentExists(string governmentId);
        Task<bool> UpdateStudentAsync(int studentId, StudentDto studentDto);
        Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateDataAsync();

        #endregion
    }
}
