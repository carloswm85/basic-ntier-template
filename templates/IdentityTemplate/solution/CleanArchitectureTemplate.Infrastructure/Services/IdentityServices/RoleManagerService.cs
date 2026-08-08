using CleanArchitectureTemplate.Infrastructure.Interfaces.IdentityInterfaces;
using CleanArchitectureTemplate.Infrastructure.Model.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Infrastructure.Services.IdentityServices;

public class RoleManagerService : IRoleManagerService
{
    #region From AdminController.cs

    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleManagerService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IQueryable<ApplicationRole> Roles => _roleManager.Roles;

    public async Task<ApplicationRole?> FindByIdAsync(string roleId)
    {
        return await _roleManager.FindByIdAsync(roleId);
    }

    public async Task<IdentityResult> CreateAsync(ApplicationRole role)
    {
        return await _roleManager.CreateAsync(role);
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationRole role)
    {
        return await _roleManager.UpdateAsync(role);
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationRole role)
    {
        return await _roleManager.DeleteAsync(role);
    }

    #endregion
}
