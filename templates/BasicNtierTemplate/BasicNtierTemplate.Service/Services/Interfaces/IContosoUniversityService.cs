using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Models;

namespace BasicNtierTemplate.Service.Services.Interfaces
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
        Task<int> CreateStudentAsync(StudentDto student);
        bool StudentExists(int studentId);
        bool StudentExists(string governmentId);
        Task<bool> UpdateStudentAsync(int studentId, StudentDto studentDto);
        Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateDataAsync();

        #endregion
    }
}
