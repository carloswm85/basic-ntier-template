using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    /// <summary>
    /// Use: This is a system-facing, lower-level operation (data creation).
    /// Intended audience: Internal systems or admins
    /// </summary>
    public interface IUserService
    {
        Task<ApplicationUserDto?> GetByIdAsync(Guid userId);
        Task<ApplicationUserDto?> GetByEmailAsync(string email);
        Task<IEnumerable<ApplicationUserDto>> GetAllAsync();
        Task<Result> CreateAsync(CreateUserRequest request);
        Task<Result> UpdateAsync(UpdateUserRequest request);
        Task<Result> DeleteAsync(Guid userId);
        Task<Result> ActivateAsync(Guid userId);
        Task<Result> DeactivateAsync(Guid userId);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
