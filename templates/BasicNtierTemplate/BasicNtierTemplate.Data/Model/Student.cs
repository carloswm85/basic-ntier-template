using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BasicNtierTemplate.Data.Model
{
    public partial class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Government Id is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        [StringLength(20)]
        [Display(Name = "Government ID")]
        public required string GovernmentId { get; set; }

        // For display purposes only
        [JsonIgnore]
        public string? GovernmentIdFormatted =>
            long.TryParse(GovernmentId, out long num)
                ? num.ToString("#,###,###", new System.Globalization.NumberFormatInfo
                {
                    NumberGroupSeparator = ".",
                    NumberDecimalDigits = 0
                })
                : null;

        [Required]
        [StringLength(80, MinimumLength = 2)]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-zA-ZÁÉÍÓÚÑáéíóúñ\s'-]*$")]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-zA-ZÁÉÍÓÚÑáéíóúñ\s'-]*$")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public required string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public List<Enrollment> Enrollments { get; set; } = [];
    }
}
