using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Web.MVC.Models.ViewModels.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
        )
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string? returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                // Log failed attempt for non-existent user
                _logger.LogWarning(
                    "Failed login attempt for non-existent email: {Email} from IP: {IpAddress}",
                    model.Email,
                    HttpContext.Connection.RemoteIpAddress?.ToString());

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogInformation(
                    "Login attempt for unconfirmed email: {UserId} ({Email})",
                    user.Id,
                    model.Email);

                ModelState.AddModelError(string.Empty, "Please confirm your email before logging in.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user: user,
                password: model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation(
                    "User {UserId} ({Email}, {UserName}) logged in successfully from IP: {IpAddress}",
                    user.Id,
                    model.Email,
                    user.UserName,
                    HttpContext.Connection.RemoteIpAddress?.ToString());

                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl)
                    : RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning(
                    "User account locked out: {UserId} ({Email}) from IP: {IpAddress}",
                    user.Id,
                    model.Email,
                    HttpContext.Connection.RemoteIpAddress?.ToString());

                return View("AccountLocked");
            }

            if (result.RequiresTwoFactor)
            {
                _logger.LogInformation(
                    "User {UserId} ({Email}) requires two-factor authentication",
                    user.Id,
                    model.Email);

                return RedirectToAction("LoginWith2fa", new { returnUrl, model.RememberMe });
            }

            // Failed password attempt
            _logger.LogWarning(
                "Failed login attempt for user: {UserId} ({Email}) from IP: {IpAddress}. Failed attempts: {FailedAttempts}",
                user.Id,
                model.Email,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                user.AccessFailedCount + 1);

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    // Log the password reset link
                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}