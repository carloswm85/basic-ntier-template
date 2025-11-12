using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BasicNtierTemplate.Service.Services
{
    public class ContosoUniversityService : IContosoUniversityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContosoUniversityService> _logger;

        public ContosoUniversityService(
            ILogger<ContosoUniversityService> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Student?> GetStudentAsync(int id)
        {
            var student = await _unitOfWork.StudentRepository.GetAll()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return student;
        }

        public async Task<List<Student>> GetStudentListAsync()
        {
            return await _unitOfWork.StudentRepository.GetAll().ToListAsync();
        }

        public async Task SaveStudentAsync(Student student)
        {
            try
            {
                _unitOfWork.StudentRepository.Add(student);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (DbUpdateException dbuex)
            {
                _logger.LogError(dbuex, "An error occurred while saving the student.");
                throw;
            }
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            try
            {
                _unitOfWork.StudentRepository.Update(student);
                await _unitOfWork.SaveChangesAsync();
                return student;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public bool StudentExists(int id)
        {
            return _unitOfWork.StudentRepository.GetAll().Any(e => e.Id == id);
        }

        public async Task DeleteStudent(int id)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id);
            if (student != null)
            {
                _unitOfWork.StudentRepository.Delete(student);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
