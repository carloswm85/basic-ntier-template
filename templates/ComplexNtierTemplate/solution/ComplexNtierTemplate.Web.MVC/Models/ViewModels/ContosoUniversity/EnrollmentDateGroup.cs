using System.ComponentModel.DataAnnotations;

namespace ComplexNtierTemplate.Web.MVC.Models.ViewModels.Student
{
    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}
