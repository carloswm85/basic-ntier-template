namespace CleanArchitectureTemplate.ApplicationCore.Entities.ContosoUniversity;

public partial class Student : IEntity<int>
{
    public int Id => StudentId;
}
