namespace CleanArchitectureTemplate.Web.Models.ViewModels.IdentityViewModels.AdminViewModels;

// DELETE?
public class UserRoleViewModel
{
    public string RoleId { get; set; } = default!;
    public string RoleName { get; set; } = default!;
    public bool Selected { get; set; }
}
