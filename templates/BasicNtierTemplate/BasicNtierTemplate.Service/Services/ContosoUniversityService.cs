using AutoMapper;
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
        private readonly IMapper _mapper;


        public ContosoUniversityService(
            ILogger<ContosoUniversityService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<StudentDto?> GetStudentAsync(int studentId, bool asNoTracking = false)
        {
            Student? student;

            if (asNoTracking)
            {
                student = await _unitOfWork.StudentRepository.GetByIdAsync(id: studentId, asNoTracking);
                return _mapper.Map<StudentDto>(student);

            }

            student = await _unitOfWork.StudentRepository.GetAll()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == studentId);

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<IEnumerable<StudentDto>> GetStudentListAsync()
        {
            var students = await _unitOfWork.StudentRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<PaginatedList<StudentDto>> GetStudentListAsync(
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

            var count = await students.CountAsync();
            var items = await students
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
                _unitOfWork.StudentRepository.Add(student);
                await _unitOfWork.SaveChangesAsync();
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
            _unitOfWork.StudentRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public bool StudentExists(int studentId)
        {
            return _unitOfWork.StudentRepository.GetAll().Any(s => s.Id == studentId);
        }

        public bool StudentExists(string governmentId)
        {
            return _unitOfWork.StudentRepository.GetAll().Any(s => s.GovernmentId.Equals(governmentId));
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            if (studentId <= 0)
                return false;

            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id: studentId, asNoTracking: true);

            if (student == null)
                return false;

            _unitOfWork.StudentRepository.Delete(student);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateDataAsync()
        {
            var students = _unitOfWork.StudentRepository.GetAll(asNoTracking: true);

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
