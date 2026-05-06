using AutoMapper;
using ComplexNLayerTemplate.Data.Model;
using ComplexNLayerTemplate.Repository;
using ComplexNLayerTemplate.Service.Dtos.ContosoUniversity;
using ComplexNLayerTemplate.Service.Services.ExampleServices;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ComplexNLayerTemplate.Testing.Unit.Services;

public class ContosoUniversityServiceUnitTests
{
    [Fact]
    public async Task GetStudentAsync_AsNoTracking_ReturnsMappedStudentDto()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Student>>();
        var mockUow = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<ContosoUniversityService>>();

        var student = new Student
        {
            Id = 1,
            GovernmentId = "12345678",
            LastName = "Doe",
            FirstMidName = "John"
        };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
        mockUow.Setup(u => u.StudentRepository).Returns(mockRepo.Object);

        var expectedDto = new StudentDto { Id = 1, GovernmentId = "12345678", LastName = "Doe", FirstMidName = "John" };
        mockMapper.Setup(m => m.Map<StudentDto>(It.IsAny<Student>())).Returns((Student s) => new StudentDto
        {
            Id = s.Id,
            GovernmentId = s.GovernmentId,
            LastName = s.LastName,
            FirstMidName = s.FirstMidName
        });

        var service = new ContosoUniversityService(mockLogger.Object, mockUow.Object, mockMapper.Object);

        // Act
        var result = await service.GetStudentAsync(1, asNoTracking: true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result!.Id);
        Assert.Equal(expectedDto.GovernmentId, result.GovernmentId);
        mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateStudentAsync_AddsStudentAndReturnsId()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Student>>();
        var mockUow = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<ContosoUniversityService>>();

        var dto = new StudentDto { GovernmentId = "ABC-12-3456", LastName = "Smith", FirstMidName = "Anna" };
        var mappedStudent = new Student { GovernmentId = "ABC-12-3456", LastName = "Smith", FirstMidName = "Anna" };

        mockMapper.Setup(m => m.Map<Student>(It.IsAny<StudentDto>())).Returns(mappedStudent);

        // When AddAsync is called, set the Id to simulate DB behaviour
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Student>()))
            .Returns(Task.CompletedTask)
            .Callback<Student>(s => s.Id = 42);

        mockUow.Setup(u => u.StudentRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new ContosoUniversityService(mockLogger.Object, mockUow.Object, mockMapper.Object);

        // Act
        var createdId = await service.CreateStudentAsync(dto);

        // Assert
        Assert.Equal(42, createdId);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
        mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
