using CleanArchitectureTemplate.ApplicationCore.Common.Pagination;
using CleanArchitectureTemplate.ApplicationCore.Constants;
using CleanArchitectureTemplate.Infrastructure.Interfaces.IdentityInterfaces;
using CleanArchitectureTemplate.Infrastructure.Model.Identity;
using CleanArchitectureTemplate.Web.Models;
using CleanArchitectureTemplate.Web.Models.ViewModels.IdentityViewModels.AdminViewModels;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Web.Controllers.IdentityControllers;

[Authorize]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IUserManagerService _userManagerService;
    private readonly IRoleManagerService _roleManagerService;

    public AdminController(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IUserManagerService userManagerService,
        IRoleManagerService roleManagerService
    )
    {
        _logger = loggerFactory.CreateLogger<AdminController>();
        _mapper = mapper;

        _userManagerService = userManagerService;
        _roleManagerService = roleManagerService;
    }

    #region CRUD

    //
    // GET Admin/Index
    public IActionResult Index()
    {
        return View();
    }

    //
    // GET Admin/List
    [HttpGet]
    public async Task<IActionResult> UserList(
        string currentFilter = "",
        int pageIndex = 1,
        int pageSize = 10,
        string searchString = "",
        string sortOrder = ""
    )
    {
        var users = await GetUsersPaginatedListAsync(
            currentFilter,
            pageIndex,
            pageSize,
            searchString,
            sortOrder
        );

        var usersPaginateViewModel = new PaginatedListViewModel<_UserListItemViewModel>(
            paginatedList: users,
            currentFilter: searchString,
            currentSort: sortOrder,
            sortColumnOne: string.IsNullOrEmpty(sortOrder)
                ? CurrentSort.LastNameDesc
                : CurrentSort.LastNameAsc,
            sortColumnTwo: sortOrder == CurrentSort.UsernameAsc
                ? CurrentSort.UsernameDesc
                : CurrentSort.UsernameAsc,
            pageSize: pageSize
        );

        return View(usersPaginateViewModel);
    }

    // GET: /Admin/Details/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
    [HttpGet("UserDetails/{id}")]
    public async Task<IActionResult> UserDetails(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManagerService.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        var userVm = _mapper.Map<UserViewModel>(user);

        userVm.Roles = await _userManagerService.GetRolesAsync(user);
        userVm.Claims = await _userManagerService.GetClaimsAsync(user);

        return View(userVm);
    }

    // GET: /Admin/UserEdit/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
    [HttpGet("UserEdit/{id}")]
    public async Task<IActionResult> UserEdit(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManagerService.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        var userVm = _mapper.Map<UserViewModel>(user);

        userVm.Roles = await _userManagerService.GetRolesAsync(user);
        userVm.Claims = await _userManagerService.GetClaimsAsync(user);

        return View(userVm);
    }

    // POST: /Admin/UserEdit/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
    [HttpPost("UserEdit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserEdit(string id, UserViewModel userVm)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        if (id != userVm.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(userVm);

        if (string.IsNullOrWhiteSpace(userVm.PhoneNumber))
            userVm.PhoneNumberConfirmed = false;

        try
        {
            var user = await _userManagerService.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            // ---- Email change ----
            if (
                !string.IsNullOrWhiteSpace(userVm.Email)
                && !string.Equals(user.Email, userVm.Email, StringComparison.OrdinalIgnoreCase)
            )
            {
                var emailResult = await _userManagerService.SetEmailAsync(user, userVm.Email);
                if (!emailResult.Succeeded)
                {
                    foreach (var error in emailResult.Errors)
                        ModelState.AddModelError(nameof(userVm.Email), error.Description);

                    return View(userVm);
                }
            }

            // ---- Username change ----
            if (
                !string.IsNullOrWhiteSpace(userVm.UserName)
                && !string.Equals(
                    user.UserName,
                    userVm.UserName,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                var userNameResult = await _userManagerService.SetUserNameAsync(
                    user,
                    userVm.UserName
                );
                if (!userNameResult.Succeeded)
                {
                    foreach (var error in userNameResult.Errors)
                        ModelState.AddModelError(nameof(userVm.UserName), error.Description);

                    return View(userVm);
                }
            }

            if (userVm.Image != null)
                userVm = await UploadImage(userVm, "users");

            // ---- Custom fields ----
            user.City = userVm.City;
            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;
            user.PhoneNumber = userVm.PhoneNumber;

            user.EmailConfirmed = userVm.EmailConfirmed;
            user.IsActive = userVm.IsActive;
            user.LockoutEnabled = userVm.LockoutEnabled;
            user.PhoneNumberConfirmed = userVm.PhoneNumberConfirmed;
            user.TwoFactorEnabled = userVm.TwoFactorEnabled;

            user.ImagePath = userVm.ImagePath;

            var updateResult = await _userManagerService.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(userVm);
            }

            return RedirectToAction(nameof(UserList));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var exists = await _userManagerService.FindByIdAsync(userVm.Id);
            if (exists == null)
                return NotFound();

            _logger.LogWarning(
                ex,
                "Concurrency conflict while updating user ID {UserId}.",
                userVm.Id
            );

            ModelState.AddModelError(
                string.Empty,
                "The record you attempted to edit was modified by another user. Please reload and try again."
            );

            return View(userVm);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while editing the user with ID {UserId}.",
                userVm.Id
            );

            ModelState.AddModelError(
                string.Empty,
                "Unable to save changes. Try again, and if the problem persists, contact your system administrator."
            );

            return View(userVm);
        }
    }

    // GET: /Admin/Delete/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
    // Display student before deletion.
    [HttpGet("UserDelete/{id}")]
    public async Task<IActionResult> UserDelete(string id, bool? saveChangesError = false)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManagerService.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return NotFound();
        var userVm = _mapper.Map<UserViewModel>(user);

        if (saveChangesError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] =
                "Delete failed. Try again, and if the problem persists "
                + "see your system administrator.";
        }

        return View(userVm);
    }

    // POST: /Admin/Delete/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
    [HttpPost("UserDelete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserDeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManagerService.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        try
        {
            var result = await _userManagerService.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(UserList));
            }

            var errors = new List<IdentityError>();
            errors.AddRange(result.Errors);

            return BadRequest(
                new
                {
                    errors = errors.Select(e => new { code = e.Code, description = e.Description }),
                }
            );
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while deleting the user with username {0}.",
                user.UserName
            );
            return RedirectToAction(nameof(UserDelete), new { id, saveChangesError = true });
        }
    }

    // GET: /Admin/UserCreate
    [AllowAnonymous]
    [HttpGet("UserCreate")]
    public IActionResult UserCreate()
    {
        return View();
    }

    // POST: /Admin/UserCreate
    [HttpPost("UserCreate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserCreate(UserViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        try
        {
            var existingUser = await _userManagerService.FindByEmailAsync(viewModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return View(viewModel);
            }

            var existingUsername = await _userManagerService.FindByNameAsync(viewModel.UserName);
            if (existingUsername != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                return View(viewModel);
            }

            viewModel = await UploadImage(viewModel, "users");

            var user = new ApplicationUser
            {
                City = viewModel.City,
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserName = viewModel.UserName,
                EmailConfirmed = viewModel.EmailConfirmed,
                IsActive = viewModel.IsActive,
                PhoneNumber = viewModel.PhoneNumber,
                PhoneNumberConfirmed = viewModel.PhoneNumberConfirmed,
                ImagePath = viewModel.ImagePath,
            };

            var defaultPassword = ApplicationConstants.TestPassword;
            var result = await _userManagerService.CreateAsync(user, defaultPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
            }

            await _userManagerService.AddToRoleAsync(user, RoleConstants.UsersRole);

            // TODO Send confirmation email or set password reset token
            // var token = await _userManagerService.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation(
                "User {Email} created successfully by {AdminUser}",
                user.Email,
                User.Identity?.Name
            );

            TempData["SuccessMessage"] =
                $"User {user.UserName} created successfully with temporal password {defaultPassword}";
            return RedirectToAction(nameof(UserList));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "Database error occurred while creating user {Email}",
                viewModel.Email
            );
            ModelState.AddModelError(
                string.Empty,
                "Unable to save changes. "
                    + "Try again, and if the problem persists, contact your system administrator."
            );
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error occurred while creating user {Email}",
                viewModel.Email
            );
            ModelState.AddModelError(
                string.Empty,
                "An unexpected error occurred. Please try again."
            );
            return View(viewModel);
        }
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> UserRolesEdit(Guid id)
    {
        var user = await _userManagerService.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        var userRoles = await _userManagerService.GetRolesAsync(user);
        var roles = _roleManagerService.Roles.ToList();

        var viewModel = new UserRolesEditViewModel
        {
            UserId = user.Id,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            UserName = user.UserName!,
            Email = user.Email,
            Roles = roles
                .Select(r => new UserRoleEditItemViewModel
                {
                    RoleId = r.Id,
                    RoleName = r.Name!,
                    IsAssigned = userRoles.Contains(r.Name!),
                })
                .ToList(),
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserRolesEdit(UserRolesEditViewModel viewModel)
    {
        var user = await _userManagerService.FindByIdAsync(viewModel.UserId);
        if (user == null)
            return NotFound();

        var currentRoles = await _userManagerService.GetRolesAsync(user);

        var selectedRoles = viewModel
            .Roles.Where(r => r.IsAssigned)
            .Select(r => r.RoleName)
            .ToList();

        var rolesToAdd = selectedRoles.Except(currentRoles);
        var rolesToRemove = currentRoles.Except(selectedRoles);

        if (rolesToAdd.Any())
            await _userManagerService.AddToRolesAsync(user, rolesToAdd);

        if (rolesToRemove.Any())
            await _userManagerService.RemoveFromRolesAsync(user, rolesToRemove);

        return RedirectToAction(nameof(UserDetails), new { id = viewModel.UserId });
    }

    #region User Roles

    [HttpGet]
    public async Task<IActionResult> RoleList()
    {
        var roles = await _roleManagerService.Roles.ToListAsync();
        return View(roles);
    }

    // GET: /Admin/RoleCreate
    [HttpGet("RoleCreate")]
    public IActionResult RoleCreate()
    {
        return View(new RoleViewModel());
    }

    // POST: /Admin/RoleCreate
    [HttpPost("RoleCreate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RoleCreate(RoleViewModel roleVm)
    {
        if (!ModelState.IsValid)
            return View(roleVm);

        try
        {
            var role = _mapper.Map<ApplicationRole>(roleVm);
            role.Id = Guid.NewGuid().ToString();

            var result = await _roleManagerService.CreateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(roleVm);
            }

            return RedirectToAction(nameof(RoleList));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating role {RoleName}.", roleVm.Name);

            ModelState.AddModelError(
                string.Empty,
                "Unable to create the role. Try again, and if the problem persists, contact your system administrator."
            );

            return View(roleVm);
        }
    }

    // GET: /Admin/RoleEdit/{id}
    [HttpGet("RoleEdit/{id}")]
    public async Task<IActionResult> RoleEdit(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var role = await _roleManagerService.FindByIdAsync(id);
        if (role == null)
            return NotFound();

        var vm = _mapper.Map<RoleViewModel>(role);

        return View(vm);
    }

    // POST: /Admin/RoleEdit/{id}
    [HttpPost("RoleEdit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RoleEdit(string id, RoleViewModel roleVm)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        if (id != roleVm.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(roleVm);

        try
        {
            var role = await _roleManagerService.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            // Map editable fields
            _mapper.Map(roleVm, role);

            var updateResult = await _roleManagerService.UpdateAsync(role);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(roleVm);
            }

            return RedirectToAction(nameof(RoleList));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var exists = await _roleManagerService.FindByIdAsync(roleVm.Id);
            if (exists == null)
                return NotFound();

            _logger.LogWarning(
                ex,
                "Concurrency conflict while updating role ID {RoleId}.",
                roleVm.Id
            );

            ModelState.AddModelError(
                string.Empty,
                "The role was modified by another user. Please reload and try again."
            );

            return View(roleVm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while editing role ID {RoleId}.", roleVm.Id);

            ModelState.AddModelError(
                string.Empty,
                "Unable to save changes. Try again, and if the problem persists, contact your system administrator."
            );

            return View(roleVm);
        }
    }

    // GET: /Admin/RoleDelete/{id}
    [HttpGet("RoleDelete/{id}")]
    public async Task<IActionResult> RoleDelete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var role = await _roleManagerService.FindByIdAsync(id);
        if (role == null)
            return NotFound();

        var vm = _mapper.Map<RoleViewModel>(role);

        return View(vm);
    }

    // POST: /Admin/RoleDelete/{id}
    [HttpPost("RoleDelete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RoleDeleteConfirmed(string id, RoleViewModel roleVm)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        if (id != roleVm.Id)
            return BadRequest();

        try
        {
            var role = await _roleManagerService.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var result = await _roleManagerService.DeleteAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View("RoleDelete", roleVm);
            }

            return RedirectToAction(nameof(RoleList));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting role ID {RoleId}.", roleVm.Id);

            ModelState.AddModelError(
                string.Empty,
                "Unable to delete the role. Try again, and if the problem persists, contact your system administrator."
            );

            return View("RoleDelete", roleVm);
        }
    }

    #endregion

    [HttpGet("Placeholder")]
    public IActionResult Placeholder(Guid id)
    {
        return Ok("Placeholder endpoint, for testing purposes only.");
    }

    #region Private Methods

    private async Task<PaginatedList<_UserListItemViewModel>> GetUsersPaginatedListAsync(
        string currentFilter,
        int pageIndex,
        int pageSize,
        string searchString,
        string sortOrder
    )
    {
        var users = _userManagerService.Users.AsQueryable();

        var totalRecords = users.Count();

        // PAGING
        if (searchString != currentFilter)
            pageIndex = 1;
        else
            searchString = currentFilter;

        // SEARCH
        if (!string.IsNullOrEmpty(searchString))
        {
            var term = searchString.Trim();

            users = users.Where(u =>
                (u.LastName != null && u.LastName.Contains(term))
                || (u.FirstName != null && u.FirstName.Contains(term))
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

        var usersPage = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        var result = new List<_UserListItemViewModel>();

        foreach (var user in usersPage)
        {
            var mappedUser = _mapper.Map<_UserListItemViewModel>(user);
            var roles = await _userManagerService.GetRolesAsync(user);
            mappedUser.Roles = roles;
            result.Add(mappedUser);
        }

        return new PaginatedList<_UserListItemViewModel>(
            items: result,
            count: count,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalRecords: totalRecords,
            filteredCount: filteredCount
        );
    }

    #endregion

    // REFACTOR
    private async Task<dynamic> UploadImage(dynamic imageContainer, string folderName)
    {
        if (imageContainer.Image != null)
        {
            var imageFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                folderName
            );

            // Ensure directory exists
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            // Generate filename
            string fileName =
                $"{imageContainer.Id}_{Guid.NewGuid()}{Path.GetExtension(imageContainer.Image.FileName)}";

            var filePath = Path.Combine(imageFolder, fileName);

            // Save file
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await imageContainer.Image.CopyToAsync(fileStream);

            // ✅ Store ONLY relative path
            imageContainer.ImagePath = $"/images/{folderName}/{fileName}";
        }
        else
        {
            // ✅ Also relative
            imageContainer.ImagePath = $"/images/defaults/default-{folderName}.png";
        }

        return imageContainer;
    }
}
