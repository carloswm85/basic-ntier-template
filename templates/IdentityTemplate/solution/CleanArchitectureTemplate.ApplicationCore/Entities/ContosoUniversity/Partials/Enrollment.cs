namespace CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;

public partial class Enrollment : IEntity<int>
{
    public int Id => EnrollmentId;
}
