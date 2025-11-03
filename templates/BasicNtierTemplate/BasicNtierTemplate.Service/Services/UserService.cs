using AutoMapper;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetByIdAsync(Guid userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());

            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<Result> CreateAsync(CreateUserRequest request)
        {
            if (await ExistsByEmailAsync(request.Email))
                return Result.Fail("A user with this email already exists.");

            var user = new ApplicationUser
            {
                City = request.City,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result.Fail(string.Join("; ", result.Errors.Select(e => e.Description)));

            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return Result.Fail("User not found.");

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.UserName = request.FullName; // Or handle in custom ApplicationUser.FullName

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result.Fail(string.Join("; ", updateResult.Errors.Select(e => e.Description)));

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

                if (!passResult.Succeeded)
                    return Result.Fail(string.Join("; ", passResult.Errors.Select(e => e.Description)));
            }

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result.Fail("User not found.");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded
                ? Result.Ok()
                : Result.Fail(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task<Result> ActivateAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result.Fail("User not found.");

            // Example: custom IsActive field on ApplicationUser (extend IdentityUser)
            if (user is IActivatable activatable)
            {
                activatable.IsActive = true;
                await _userManager.UpdateAsync(user);
                return Result.Ok();
            }

            return Result.Fail("User model does not support activation.");
        }

        public async Task<Result> DeactivateAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result.Fail("User not found.");

            if (user is IActivatable activatable)
            {
                activatable.IsActive = false;
                await _userManager.UpdateAsync(user);
                return Result.Ok();
            }

            return Result.Fail("User model does not support deactivation.");
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
    }

    /// <summary>
    /// Optional interface if your ApplicationUser supports activation/deactivation.
    /// </summary>
    public interface IActivatable
    {
        bool IsActive { get; set; }
    }
}
