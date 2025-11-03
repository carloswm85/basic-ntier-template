using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ILogger<AccountController> logger,
            IRegistrationService registrationService
        )
        {

            _logger = logger;
            _registrationService = registrationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registerRequest = new RegisterRequest()
                    {
                        City = model.City,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.UserName,
                        //ConfirmPassword = model.ConfirmPassword,
                    };

                    var result = await _registrationService.RegisterAsync(registerRequest);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("User registered successfully.");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    _logger.LogError(httpEx, "HTTP request error during user registration.");
                    ModelState.AddModelError(string.Empty, "A network error occurred. Please try again later.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred during user registration.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            return View();
        }
    }
}