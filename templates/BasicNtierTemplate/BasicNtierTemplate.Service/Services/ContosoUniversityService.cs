using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Dtos;
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

        public async Task<Student?> GetStudentAsync(int id, bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return await _unitOfWork.StudentRepository.GetByIdAsync(id: id, asNoTracking);
            }

            var student = await _unitOfWork.StudentRepository.GetAll()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return student;
        }

        public async Task<IEnumerable<Student>> GetStudentListAsync()
        {
            return await _unitOfWork.StudentRepository.GetAll().ToListAsync();
        }

        public async Task<PaginatedList<Student>> GetStudentListAsync(
            string currentFilter,
            int pageIndex,
            int pageSize,
            string searchString,
            string sortOrder
        )
        {
            var students = _unitOfWork.StudentRepository.GetAll(asNoTracking: true);
            var totalRecords = students.Count();

            // PAGING
            if (searchString != currentFilter)
                pageIndex = 1;
            else
                searchString = currentFilter;

            // SEARCH
            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                        || s.FirstMidName.Contains(searchString));
            }
            var filteredCount = students.Count();

            // SORTING
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            return await PaginatedList<Student>
                .CreateAsync(students, pageIndex, pageSize, totalRecords, filteredCount);
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

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            try
            {
                _unitOfWork.StudentRepository.Update(student);
                await _unitOfWork.SaveChangesAsync();
                return student;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public bool StudentExists(int id)
        {
            return _unitOfWork.StudentRepository.GetAll().Any(e => e.Id == id);
        }

        public async Task<Student?> DeleteStudentAsync(int id)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id: id, asNoTracking: true);

            if (student != null)
            {
                _unitOfWork.StudentRepository.Delete(student);
                await _unitOfWork.SaveChangesAsync();
                return student;
            }

            return null;
        }
    }
}
