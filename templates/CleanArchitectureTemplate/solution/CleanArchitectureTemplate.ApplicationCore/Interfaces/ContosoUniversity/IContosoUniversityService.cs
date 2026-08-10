using CleanArchitectureTemplate.ApplicationCore.Dtos.ContosoUniversity;
using CleanArchitectureTemplate.ApplicationCore.Entities;

namespace CleanArchitectureTemplate.ApplicationCore.Interfaces.ContosoUniversity;

public interface IContosoUniversityService
{
    #region Student

    Task<bool> DeleteStudentAsync(int studentId);
    Task<StudentDto?> GetStudentAsync(int studentId, bool asNoTracking = false);
    Task<IEnumerable<StudentDto>> GetStudentListAsync();
    Task<int> CreateStudentAsync(StudentDto studentDto);
    bool StudentExists(int studentId);
    bool StudentExists(string governmentId);
    Task<bool> UpdateStudentAsync(int studentId, StudentDto studentDto);
    Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateDataAsync();

    #endregion

    #region Contact

    Task<IEnumerable<Contact>> GetContactsAsync(bool isAuthorized, string currentUserId);
    Task<Contact?> GetByIdAsync(int id);
    Task<Contact?> GetByIdAsNoTrackingAsync(int id);
    Task CreateAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task UpdateStatusAsync(int id, ContactStatus status);
    Task DeleteAsync(Contact contact);

    #endregion
}
