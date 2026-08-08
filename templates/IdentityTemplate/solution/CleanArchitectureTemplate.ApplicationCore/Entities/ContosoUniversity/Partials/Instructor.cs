namespace CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;

public partial class Instructor : IEntity<int>
{
    public int Id => InstructorId;
}
