using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IContosoUniversityService
    {
        Task<Student?> GetStudentDetailsAsync(int id);
        Task<List<Student>> GetStudentsAsync();
        Task SaveStudentAsync(Student student);
    }
}
