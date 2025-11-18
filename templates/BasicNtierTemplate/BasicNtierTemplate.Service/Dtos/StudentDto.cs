using System.ComponentModel.DataAnnotations;

namespace BasicNtierTemplate.Service.Dtos
{
    public class StudentDto
    {
        private string _governmentId = string.Empty;

        public int Id { get; set; }

        [Required(ErrorMessage = "Government Id is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        public string GovernmentId
        {
            get => _governmentId;
            set
            {
                // Ensure null-safe assignment
                if (string.IsNullOrWhiteSpace(value))
                {
                    _governmentId = string.Empty;
                    return;
                }

                // Strip non-digits and assign
                _governmentId = new string(value.Where(char.IsDigit).ToArray());
            }
        }

        public string? GovernmentIdFormatted =>
            long.TryParse(GovernmentId, out long num)
                ? num.ToString("#,###,###", new System.Globalization.NumberFormatInfo
                {
                    NumberGroupSeparator = ".",
                    NumberDecimalDigits = 0
                })
                : null;

        public required string LastName { get; set; }
        public required string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public List<EnrollmentDto> Enrollments { get; set; } = [];
    }
}
