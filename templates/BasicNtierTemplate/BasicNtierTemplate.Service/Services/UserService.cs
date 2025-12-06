using AutoMapper;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Models;
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

        public async Task<ApplicationUserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user is null ? null : _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<ApplicationUserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null ? null : _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDto>>(users);
        }

        public async Task<PaginatedList<ApplicationUserDto>> GetUsersPaginatedListAsync(
            string currentFilter,
            int pageIndex,
            int pageSize,
            string searchString,
            string sortOrder
        )
        {
            var users = _userManager.Users.AsQueryable();
            var totalRecords = users.Count();

            // PAGING
            if (searchString != currentFilter)
                pageIndex = 1;
            else
                searchString = currentFilter;

            // SEARCH
            if (!string.IsNullOrEmpty(searchString))
            {
                var term = searchString.Trim().ToUpper();

                users = users.Where(u =>
                    u.LastName.ToUpper().Contains(term) ||
                    u.FirstName.ToUpper().Contains(term)
                );
            }
            var filteredCount = users.Count();

            // SORTING
            switch (sortOrder)
            {
                case CurrentSort.LastNameDesc:
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                case CurrentSort.UsernameAsc:
                    users = users.OrderBy(s => s.UserName);
                    break;
                case CurrentSort.UsernameDesc:
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }

            var count = users.Count();
            var items = users
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var usersDto = _mapper.Map<List<ApplicationUserDto>>(items);

            return new PaginatedList<ApplicationUserDto>(
                items: usersDto,
                count: count,
                pageIndex: pageIndex,
                pageSize: pageSize,
                totalRecords: totalRecords,
                filteredCount: filteredCount
            );
        }

        public async Task<OperationResult> CreateUserAsync(CreateUserCommand request)
        {
            if (await ExistsUserByEmailAsync(request.Email))
                return OperationResult.Fail("A user with this email already exists.");

            var user = new ApplicationUser
            {
                City = request.City,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return OperationResult.Fail(string.Join("; ", result.Errors.Select(e => e.Description)));

            return OperationResult.Ok(data: user.Id);
        }

        public async Task<OperationResult> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return OperationResult.Fail("User not found.");

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.UserName = request.FullName; // Or handle in custom ApplicationUser.FullName

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return OperationResult.Fail(string.Join("; ", updateResult.Errors.Select(e => e.Description)));

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

                if (!passResult.Succeeded)
                    return OperationResult.Fail(string.Join("; ", passResult.Errors.Select(e => e.Description)));
            }

            return OperationResult.Ok(data: user.Id);
        }

        public async Task<OperationResult> UpdateUserAsync(ApplicationUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null)
                return OperationResult.Fail("User not found.");

            var errors = new List<IdentityError>();

            if (!string.IsNullOrWhiteSpace(userDto.Email) && user.Email != userDto.Email)
            {
                var result = await _userManager.SetEmailAsync(user, userDto.Email);
                if (!result.Succeeded)
                    errors.AddRange(result.Errors);
            }

            if (!string.IsNullOrWhiteSpace(userDto.UserName) && user.UserName != userDto.UserName)
            {
                var result = await _userManager.SetUserNameAsync(user, userDto.UserName);
                if (!result.Succeeded)
                    errors.AddRange(result.Errors);
            }

            if (errors.Any())
                return OperationResult.Fail(string.Join("; ", errors.Select(e => e.Description)));

            user.City = userDto.City;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            var updateResult = await _userManager.UpdateAsync(user);


            if (!updateResult.Succeeded)
                return OperationResult.Fail(string.Join("; ", updateResult.Errors.Select(e => e.Description)));

            return OperationResult.Ok(data: user.Id);
        }


        public async Task<OperationResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return OperationResult.Fail("User not found.");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded
                ? OperationResult.Ok(data: user.Id)
                : OperationResult.Fail(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task<OperationResult> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return OperationResult.Fail("User not found.");

            // Example: custom IsActive field on ApplicationUser (extend IdentityUser)
            if (user is IActivatable activatable)
            {
                activatable.IsActive = true;
                await _userManager.UpdateAsync(user);
                return OperationResult.Ok(data: user.Id);
            }

            return OperationResult.Fail("User model does not support activation.");
        }

        public async Task<OperationResult> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return OperationResult.Fail("User not found.");

            if (user is IActivatable activatable)
            {
                activatable.IsActive = false;
                await _userManager.UpdateAsync(user);
                return OperationResult.Ok(data: user.Id);
            }

            return OperationResult.Fail("User model does not support deactivation.");
        }

        public async Task<bool> ExistsUserByEmailAsync(string email)
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
