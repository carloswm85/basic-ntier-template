using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos.User;
using BasicNtierTemplate.Service.Models;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    /// <summary>
    /// Use: This is a system-facing, lower-level operation (data creation).
    /// Intended audience: Internal systems or admins
    /// </summary>
    public interface IUserService
    {
        Task<ApplicationUserDto?> GetUserByIdAsync(string userId);
        Task<ApplicationUserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync();

        Task<PaginatedList<ListedApplicationUserDto>> GetUsersPaginatedListAsync(
            string currentFilter, int pageIndex, int pageSize,
            string searchString, string sortOrder);

        Task<OperationResult> CreateUserAsync(CreateUserRequest request);
        Task<OperationResult> UpdateUserAsync(UpdateUserRequest request);
        Task<OperationResult> UpdateUserAsync(ApplicationUserDto userDto);
        Task<OperationResult> DeleteUserAsync(string userId);
        Task<OperationResult> ActivateUserAsync(string userId);
        Task<OperationResult> DeactivateUserAsync(string userId);
        Task<bool> ExistsUserByEmailAsync(string email);
    }
}
