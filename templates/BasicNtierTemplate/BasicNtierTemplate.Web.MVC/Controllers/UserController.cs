using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<RegistrationController> _logger;

        public UserController(
            ILogger<RegistrationController> logger,
            IUserService userService
        )
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

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

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Details(string username)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Edit(string username)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult ToggleActive(string username)
        {
            // enable/disable logic here
            return RedirectToAction(nameof(Index));
        }


        public Task<Result> CreateAsync(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ActivateAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeactivateAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}