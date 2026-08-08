using System.ComponentModel.DataAnnotations;

namespace SimpleMonolithTemplate.Monolith.Models.ViewModels.ContosoUniversity;

public class EnrollmentDateGroup
{
    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }

    public int StudentCount { get; set; }
}
