namespace CleanArchitectureTemplate.Web.Models.ViewModels.IdentityViewModels.AdminViewModels;

public class UserRoleEditItemViewModel
{
    public string RoleId { get; set; } = default!;
    public string RoleName { get; set; } = default!;
    public bool IsAssigned { get; set; }
}
