using CleanArchitectureTemplate.ApplicationCore.Dtos.ContosoUniversity;
using CleanArchitectureTemplate.ApplicationCore.Entities;
using CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;
using CleanArchitectureTemplate.ApplicationCore.Interfaces.ContosoInterfaces;
using CleanArchitectureTemplate.Infrastructure.Model;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureTemplate.Infrastructure.Services.ContosoServices;

public class ContosoUniversityService : IContosoUniversityService
{
    private readonly ILogger<ContosoUniversityService> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ContosoUniversityService(
        ILogger<ContosoUniversityService> logger,
        IMapper mapper,
        ApplicationDbContext context
    )
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = context;
    }

    #region Student

    public async Task<StudentDto?> GetStudentAsync(int studentId, bool asNoTracking = false)
    {
        Student? student;

        if (asNoTracking)
        {
            student = await _dbContext.Students.FindAsync(studentId);

            if (student == null)
            {
                _logger.LogWarning("Student with ID {StudentId} not found.", studentId);
                return null;
            }

            return _mapper.Map<StudentDto>(student);
        }

        student = await _dbContext
            .Students.Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.StudentId == studentId);

        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found.", studentId);
            return null;
        }

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<IEnumerable<StudentDto>> GetStudentListAsync()
    {
        var students = await _dbContext.Students.ToListAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<int> CreateStudentAsync(StudentDto studentDto)
    {
        try
        {
            var student = _mapper.Map<Student>(studentDto);

            student.GovernmentId = new string(student.GovernmentId.Where(char.IsDigit).ToArray());

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return student.StudentId;
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

        student.GovernmentId = new string(student.GovernmentId.Where(char.IsDigit).ToArray());

        _dbContext.Students.Update(student);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public bool StudentExists(int studentId)
    {
        return _dbContext.Students.Any(s => s.StudentId == studentId);
    }

    public bool StudentExists(string governmentId)
    {
        return _dbContext.Students.Any(s => s.GovernmentId.Equals(governmentId));
    }

    public async Task<bool> DeleteStudentAsync(int studentId)
    {
        if (studentId <= 0)
            return false;

        var student = await _dbContext.Students.FindAsync(studentId);

        if (student == null)
            return false;

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<EnrollmentDateGroupDto>> GetEnrollmentDateDataAsync()
    {
        var students = _dbContext.Students.AsQueryable();

        IQueryable<EnrollmentDateGroupDto> data =
            from student in students
            group student by student.EnrollmentDate.Year into dateGroup
            select new EnrollmentDateGroupDto()
            {
                EnrollmentYear = dateGroup.Key,
                StudentCount = dateGroup.Count(),
            };

        return await data.ToListAsync();
    }

    #endregion

    #region Contact

    public async Task<IEnumerable<Contact>> GetContactsAsync(
        bool isAuthorized,
        string currentUserId
    )
    {
        var contacts = _dbContext.Contact.AsQueryable();

        if (!isAuthorized)
        {
            contacts = contacts.Where(c =>
                c.Status == ContactStatus.Approved || c.OwnerID == currentUserId
            );
        }

        return await contacts.ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await _dbContext.Contact.FirstOrDefaultAsync(c => c.ContactId == id);
    }

    public async Task<Contact?> GetByIdAsNoTrackingAsync(int id)
    {
        return await _dbContext.Contact.AsNoTracking().FirstOrDefaultAsync(c => c.ContactId == id);
    }

    public async Task CreateAsync(Contact contact)
    {
        _dbContext.Contact.Add(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        _dbContext.Attach(contact).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, ContactStatus status)
    {
        var contact = await GetByIdAsync(id);

        if (contact == null)
            return;

        contact.Status = status;
        _dbContext.Contact.Update(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Contact contact)
    {
        _dbContext.Contact.Remove(contact);
        await _dbContext.SaveChangesAsync();
    }

    #endregion
}
