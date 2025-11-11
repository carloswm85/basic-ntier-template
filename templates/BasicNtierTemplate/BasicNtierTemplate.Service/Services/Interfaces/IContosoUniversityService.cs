using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IContosoUniversityService
    {
        Task<List<Student>> GetStudentsAsync();
    }
}
