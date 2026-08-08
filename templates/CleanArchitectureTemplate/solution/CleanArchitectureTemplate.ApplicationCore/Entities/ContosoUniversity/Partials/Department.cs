namespace CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;

public partial class Department : IEntity<int>
{
    public int Id => DepartmentId;
}
