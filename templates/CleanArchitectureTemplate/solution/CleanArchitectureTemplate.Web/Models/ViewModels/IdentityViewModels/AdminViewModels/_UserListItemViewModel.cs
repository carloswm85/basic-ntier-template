using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureTemplate.Web.Models.ViewModels.IdentityViewModels.AdminViewModels;

public class _UserListItemViewModel
{
    public _UserListItemViewModel() { }

    public Guid Id { get; set; } = default!;

    [Display(Name = "Username")]
    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;

    [Display(Name = "First name")]
    public string FirstName { get; set; } = default!;

    [Display(Name = "Last name")]
    public string LastName { get; set; } = default!;

    [Display(Name = "Confirmed email")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Active user")]
    public bool IsActive { get; set; }

    [Display(Name = "Lockout enabled")]
    public bool LockoutEnabled { get; set; }

    [Display(Name = "Is lockedout")]
    public DateTimeOffset? LockoutEnd { get; set; }

    [Display(Name = "Roles")]
    public IList<string> Roles { get; set; } = new List<string>();
}
