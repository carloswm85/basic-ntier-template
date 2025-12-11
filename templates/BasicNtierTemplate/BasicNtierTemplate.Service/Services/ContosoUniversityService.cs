using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Dtos.Contoso;
using BasicNtierTemplate.Service.Models;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BasicNtierTemplate.Service.Services
{
    public class ContosoUniversityService : IContosoUniversityService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ContosoUniversityService> _logger;
        private readonly IMapper _mapper;


        public ContosoUniversityService(
            ILogger<ContosoUniversityService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _logger = logger;
            _uow = unitOfWork;
            _mapper = mapper;

        }

        public async Task<StudentDto?> GetStudentAsync(int studentId, bool asNoTracking = false)
        {
            Student? student;

            if (asNoTracking)
            {
                student = await _uow.StudentRepository.GetByIdAsync(studentId);
                return _mapper.Map<StudentDto>(student);

            }

            student = await _uow.StudentRepository.Query()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == studentId);

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<IEnumerable<StudentDto>> GetStudentListAsync()
        {
            var students = await _uow.StudentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<PaginatedList<StudentDto>> GetStudentsPaginatedListAsync(
            string currentFilter,
            int pageIndex,
            int pageSize,
            string searchString,
            string sortOrder
        )
        {
            var students = await _uow.StudentRepository.GetAllAsync();
            var totalRecords = students.Count();

            // PAGING
            if (searchString != currentFilter)
                pageIndex = 1;
            else
                searchString = currentFilter;

            // SEARCH
            if (!string.IsNullOrEmpty(searchString))
            {
                var term = searchString.Trim().ToUpper();

                students = students.Where(s =>
                    s.LastName.ToUpper().Contains(term) ||
                    s.FirstMidName.ToUpper().Contains(term)
                );
            }
            var filteredCount = students.Count();

            // SORTING
            switch (sortOrder)
            {
                case CurrentSort.LastNameDesc:
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case CurrentSort.DateAsc:
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case CurrentSort.DateDesc:
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            var count = students.Count();
            var items = students
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var studentsDto = _mapper.Map<List<StudentDto>>(items);

            return new PaginatedList<StudentDto>(
                items: studentsDto,
                count: count,
                pageIndex: pageIndex,
                pageSize: pageSize,
                totalRecords: totalRecords,
                filteredCount: filteredCount
            );
        }

        public async Task<int> CreateStudentAsync(StudentDto studentDto)
        {
            try
            {
                var student = _mapper.Map<Student>(studentDto);

                student.GovernmentId = new string(student.GovernmentId
                    .Where(char.IsDigit).ToArray());

                await _uow.StudentRepository.AddAsync(student);
                await _uow.SaveChangesAsync();
                return student.Id;
            }
            catch (DbUpdateException dbuex)
            {
                _logger.LogError(dbuex, "An error occurred while saving the student.");
                throw;
            }
        }

        public async Task<bool> UpdateStudentAsync(int studentId, StudentDto studentDto)
        {
            if (studentId <= 0 || studentDto == null)
                return false;

            studentDto.Id = studentId;
            var student = _mapper.Map<Student>(studentDto);

            student.GovernmentId = new string(student.GovernmentId
                    .Where(char.IsDigit).ToArray());

            _uow.StudentRepository.Update(student);
            await _uow.SaveChangesAsync();
            return true;
        }

        public bool StudentExists(int studentId)
        {
            return _uow.StudentRepository.Query().Any(s => s.Id == studentId);
        }

        public bool StudentExists(string governmentId)
        {
            return _uow.StudentRepository.Query().Any(s => s.GovernmentId.Equals(governmentId));
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            if (studentId <= 0)
                return false;

            var student = await _uow.StudentRepository.GetByIdAsync(studentId);

            if (student == null)
                return false;

            _uow.StudentRepository.Remove(student);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateDataAsync()
        {
            var students = _uow.StudentRepository.Query();

            IQueryable<EnrollmentDateGroupDto> data =
                from student in students
                group student by student.EnrollmentDate.Year into dateGroup
                select new EnrollmentDateGroupDto()
                {
                    EnrollmentYear = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };

            return await data.ToListAsync();
        }
    }
}
