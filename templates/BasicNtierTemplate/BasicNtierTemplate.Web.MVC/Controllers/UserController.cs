using AutoMapper;
using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Models;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Models;
using BasicNtierTemplate.Web.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<RegistrationController> _logger;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<RegistrationController> logger,
            IMapper mapper,
            IUserService userService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        #region CRUD

        // GET: /User/List
        [HttpGet("List")]
        public async Task<IActionResult> List(
            string currentFilter = "",
            int pageIndex = 1,
            int pageSize = 10,
            string searchString = "",
            string sortOrder = ""
        )
        {
            var users = await _userService.GetUsersPaginatedListAsync(
                currentFilter, pageIndex, pageSize, searchString, sortOrder);

            var usersPaginateViewModel = new PaginatedListViewModel<ApplicationUserDto>(
                paginatedList: users,
                currentFilter: searchString,
                currentSort: sortOrder,
                sortColumnOne: string.IsNullOrEmpty(sortOrder)
                    ? CurrentSort.LastNameDesc : CurrentSort.LastNameAsc,
                sortColumnTwo: sortOrder == CurrentSort.UsernameAsc
                    ? CurrentSort.UsernameDesc : CurrentSort.UsernameAsc,
                pageSize: pageSize
            );

            return View(usersPaginateViewModel);
        }

        // GET: /User/Details/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userService.GetUserByIdAsync(id);
            var userVm = _mapper.Map<UserViewModel>(user);

            if (user == null)
                return NotFound();

            return View(userVm);
        }

        // GET: /User/Edit/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userService.GetUserByIdAsync(id);
            var userVm = _mapper.Map<UserViewModel>(user);

            if (user == null)
                return NotFound();

            return View(userVm);
        }

        // POST: /User/Edit/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel userVm)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (!ModelState.IsValid)
                return View(userVm);

            var userDto = _mapper.Map<ApplicationUserDto>(userVm);

            try
            {
                await _userService.UpdateUserAsync(userDto);
                return RedirectToAction(nameof(List));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _userService.ExistsUserByEmailAsync(userDto.Email))
                    return NotFound();

                _logger.LogWarning(ex, "Concurrency conflict while updating usert ID {0}.", userDto.Id);
                ModelState.AddModelError("", "The record you attempted to edit was modified by another user.");
                return View(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the user with ID {0}.", userDto.Id);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                return View(userDto);
            }
        }

        // GET: /User/Delete/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
        // Display student before deletion.
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id, bool? saveChangesError = false)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            var userVm = _mapper.Map<UserViewModel>(user);

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(userVm);
        }

        // POST: /User/Delete/0066d2ba-7012-4b4a-86dd-d5a9e33ee803
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return RedirectToAction(nameof(Index));

            try
            {
                await _userService.DeleteUserAsync(id);
                return RedirectToAction(nameof(List));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with username {0}.", user.UserName);
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }

        }

        // GET: /User/Create
        [AllowAnonymous]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(UserViewModel userVm)
        {
            if (!ModelState.IsValid)
                return View(userVm);

            var newUser = _mapper.Map<CreateUserCommand>(userVm);

            try
            {
                await _userService.CreateUserAsync(newUser);
                return RedirectToAction(nameof(List));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return View(userVm);
            }
        }

        #endregion

        [HttpGet]
        public Task<UserViewModel?> GetByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<UserViewModel?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult ToggleActive(string username)
        {
            // enable/disable logic here
            return RedirectToAction(nameof(Index));
        }

        public Task<OperationResult> UpdateAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ActivateAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeactivateAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Placeholder")]
        public IActionResult Placeholder(Guid id)
        {
            return Ok("Nothing to see here");
        }
    }
}