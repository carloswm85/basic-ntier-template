using System.ComponentModel.DataAnnotations;

namespace BasicNtierTemplate.Web.MVC.Models
{
    public class RegisterViewModel
    {
        public string City { get; set; }

        [Required]
        [EmailAddress]
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        //[ValidEmailDomain(allowedDomain: "pragimtech.com",
        //    ErrorMessage = "Email domain must be pragimtech.com")]
        public string Email { get; set; }

        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Username { get; set; }
    }
}
