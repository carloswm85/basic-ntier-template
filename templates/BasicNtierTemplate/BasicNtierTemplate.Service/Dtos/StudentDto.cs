namespace BasicNtierTemplate.Service.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public required string GovernmentId { get; set; }

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

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public List<EnrollmentDto> Enrollments { get; set; } = [];
    }
}
