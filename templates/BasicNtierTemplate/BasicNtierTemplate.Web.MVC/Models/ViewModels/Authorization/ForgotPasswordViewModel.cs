using System.ComponentModel.DataAnnotations;

namespace BasicNtierTemplate.Web.MVC.Models.ViewModels.Authorization
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
