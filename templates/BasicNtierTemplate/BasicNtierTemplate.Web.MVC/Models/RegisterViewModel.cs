using System.ComponentModel.DataAnnotations;

namespace BasicNtierTemplate.Web.MVC.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "EDIT_USER_CITY_LABEL")]
        public required string City { get; set; }

        [Required(ErrorMessage = "EDIT_USER_EMAIL_REQUIRED")]
        [EmailAddress(ErrorMessage = "EDIT_USER_EMAIL_VALIDATION")]
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        //[ValidEmailDomain(allowedDomain: "mydomain.com",
        //    ErrorMessage = "Email domain must be mydomain.com")]
        public required string Email { get; set; }

        [Display(Name = "EDIT_USER_FIRSTNAME_LABEL")]
        [Required(ErrorMessage = "EDIT_USER_FIRSTNAME_REQUIRED")]
        [MaxLength(50, ErrorMessage = "EDIT_USER_FIRSTNAME_LENGTH")]
        public required string FirstName { get; set; }

        [Display(Name = "EDIT_USER_LASTNAME_LABEL")]
        [Required(ErrorMessage = "EDIT_USER_LASTNAME_REQUIRED")]
        [MaxLength(50, ErrorMessage = "EDIT_USER_LASTNAME_LENGTH")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "EDIT_USER_PASSWORD_REQUIRED")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "EDIT_USER_PASSWORD_LENGTH")]
        // Forward-thinking: Policies like length, symbols, etc. might later be controlled by IdentityOptions.
        public required string Password { get; set; }

        [Display(Name = "EDIT_USER_PHONENUMBER_LABEL")]
        [Phone(ErrorMessage = "EDIT_USER_PHONENUMBER_VALID")]
        public required string PhoneNumber { get; set; }

        [Display(Name = "EDIT_USER_PASSWORD_CONFIRM_LABEL")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "EDIT_USER_PASSWORD_CONFIRM_COMPARE_VALIDATION")]
        public required string ConfirmPassword { get; set; }

        [Display(Name = "EDIT_USER_USERNAME_LABEL")]
        [Required(ErrorMessage = "EDIT_USER_USERNAME_REQUIRED")]
        [MinLength(3, ErrorMessage = "EDIT_USER_USERNAME_MINLENGTH_VALIDATION")]
        [MaxLength(30, ErrorMessage = "EDIT_USER_USERNAME_MAXLENGTH_VALIDATION")]
        public required string UserName { get; set; }
    }
}
