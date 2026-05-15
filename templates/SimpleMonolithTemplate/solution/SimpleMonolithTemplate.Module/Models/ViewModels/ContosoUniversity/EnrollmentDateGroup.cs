using System.ComponentModel.DataAnnotations;

namespace SimpleMonolithTemplate.Module.Models.ViewModels.ContosoUniversity;

public class EnrollmentDateGroup
{
    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }

    public int StudentCount { get; set; }
}
