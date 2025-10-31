using System.ComponentModel.DataAnnotations;

namespace BasicNtierTemplate.Web.MVC.Utilities
{

    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object? value)
        {
            // If the value is null or empty, we don't validate it here.
            // Let [Required] handle mandatory checks.
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true;

            var email = value.ToString();
            var parts = email!.Split('@');

            // If the format is not a valid email structure
            if (parts.Length != 2)
                return false;

            return parts[1].Equals(allowedDomain, StringComparison.OrdinalIgnoreCase);
        }
    }
}
