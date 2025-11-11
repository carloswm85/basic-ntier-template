using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Service.Services
{
    public class ContosoUniversityService : IContosoUniversityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContosoUniversityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _unitOfWork.StudentRepository.GetAll().ToListAsync();
        }
    }
}
