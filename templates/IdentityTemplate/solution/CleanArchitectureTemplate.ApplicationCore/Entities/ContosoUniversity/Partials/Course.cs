namespace CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;

public partial class Course : IEntity<int>
{
    public int Id => CourseId;
}
