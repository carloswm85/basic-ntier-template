namespace CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;

public partial class OfficeAssignment : IEntity<int>
{
    public int Id => InstructorId;
}
