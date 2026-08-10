using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CleanArchitectureTemplate.Web.Validations;

namespace CleanArchitectureTemplate.Web.Models.ViewModels.Identity.Admin;

public class UserViewModel
{
    [Required]
    public string Id { get; set; } = default!;

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

    #region booleans

    [Display(Name = "Email Confirmed")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    [Display(Name = "Phone Confirmed")]
    public bool PhoneNumberConfirmed { get; set; }
    public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(PhoneNumber);

    [Display(Name = "Two-Factor Enabled")]
    public bool TwoFactorEnabled { get; set; }

    [Display(Name = "Lockout Enabled")]
    public bool LockoutEnabled { get; set; }

    #endregion

    #region image

    public string? ImagePath { get; set; }

    [AllowedFileExtensions([".jpg", ".jpeg", ".png", ".webp"])]
    [MaxFileSize(2 * 1024 * 1024)] // 2MB
    public IFormFile? Image { get; set; }

    #endregion

    [Phone]
    [Display(Name = "Phone Number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Lockout End")]
    public DateTimeOffset? LockoutEnd { get; set; }

    [Display(Name = "Failed Access Attempts")]
    public int AccessFailedCount { get; set; }

    [Display(Name = "Roles")]
    public IList<string> Roles { get; set; } = new List<string>();

    [Display(Name = "Claims")]
    public IList<Claim> Claims { get; set; } = new List<Claim>();
}
