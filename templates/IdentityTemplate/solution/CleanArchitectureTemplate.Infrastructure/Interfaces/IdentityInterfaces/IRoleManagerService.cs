using CleanArchitectureTemplate.Infrastructure.Model.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Infrastructure.Interfaces.IdentityInterfaces;

public interface IRoleManagerService
{
    #region From AdminController.cs

    IQueryable<ApplicationRole> Roles { get; }

    Task<ApplicationRole?> FindByIdAsync(string roleId);

    Task<IdentityResult> CreateAsync(ApplicationRole role);

    Task<IdentityResult> UpdateAsync(ApplicationRole role);

    Task<IdentityResult> DeleteAsync(ApplicationRole role);

    #endregion
}
