using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace BasicNtierTemplate.Web.MVC.Models.ViewModels.Authorization
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        // AuthenticationScheme is from Microsoft.AspNetCore.Authentication namespace
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
