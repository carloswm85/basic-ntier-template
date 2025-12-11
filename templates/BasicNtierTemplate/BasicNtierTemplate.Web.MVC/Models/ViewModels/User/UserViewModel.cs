using System.ComponentModel.DataAnnotations;
using BasicNtierTemplate.Service.Dtos.User;
using BasicNtierTemplate.Web.MVC.Models.ViewModels;

namespace BasicNtierTemplate.Web.MVC.Models.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public Guid Id { get; set; } = default!;

        [Required]
        [Display(Name = "Username")]
        [StringLength(256)]
        public string UserName { get; set; } = default!;

        [Required]
        [Display(Name = "First name")]
        [StringLength(100)]
        public string FirstName { get; set; } = default!;

        [Required]
        [Display(Name = "Last name")]
        [StringLength(100)]
        public string LastName { get; set; } = default!;

        [Display(Name = "Full Name")]
        [StringLength(200)]
        public string? FullName => $"{FirstName} {LastName}";

        [Required]
        [Display(Name = "City")]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(256)]
        public string Email { get; set; } = default!;

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Phone Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Two-Factor Enabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Lockout End")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Failed Access Attempts")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "Roles")]
        public IList<string> Roles { get; set; } = new List<string>();

        [Display(Name = "Claims")]
        public IList<string> Claims { get; set; } = new List<string>();
    }
}

// TODO should i keep this?
public static class UserExtensions
{
    public static UserViewModel ToViewModel(
        this ApplicationUserDto user,
        IList<string> roles,
        IList<string> claims)
    {
        return new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnd = user.LockoutEnd,
            LockoutEnabled = user.LockoutEnabled,
            AccessFailedCount = user.AccessFailedCount,
            Roles = roles,
            Claims = claims
        };
    }
}
