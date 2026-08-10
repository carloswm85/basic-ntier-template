// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;
using CleanArchitectureTemplate.ApplicationCore.Interfaces.Identity;
using CleanArchitectureTemplate.Infrastructure.Interfaces.IdentityInterfaces;
using CleanArchitectureTemplate.Infrastructure.Model.Identity;
using CleanArchitectureTemplate.Web.Models.ViewModels.Identity.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanArchitectureTemplate.Web.Controllers.Identity;

[Authorize]
public class AccountController(
    IUserManagerService userManagerService,
    ISignInManagerService signInManagerService,
    IEmailSender emailSender,
    ISmsSender smsSender,
    ILoggerFactory loggerFactory
) : Controller
{
    private readonly IUserManagerService _userManagerService = userManagerService;
    private readonly ISignInManagerService _signInManagerService = signInManagerService;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ISmsSender _smsSender = smsSender;
    private readonly ILogger _logger = loggerFactory.CreateLogger<AccountController>();

    //
    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = await _userManagerService.FindByEmailAsync(viewModel.Email);
            if (user != null && !user.IsActive)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account has been deactivated. Please contact support."
                );
                return View(viewModel);
            }

            var result = await _signInManagerService.PasswordSignInAsync(
                viewModel.Email,
                viewModel.Password,
                viewModel.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(
                    nameof(SendCode),
                    new { ReturnUrl = returnUrl, viewModel.RememberMe }
                );
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(viewModel);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(viewModel);
    }

    //
    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    //
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserName = viewModel.Username,
                Email = viewModel.Email,
            };

            if (string.IsNullOrWhiteSpace(viewModel.Password))
            {
                ModelState.AddModelError(nameof(viewModel.Password), "Password is required.");
                return View(viewModel);
            }

            var result = await _userManagerService.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManagerService.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(viewModel.Email, "Confirm your account",
                //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                await _signInManagerService.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(3, "User created a new account with password.");
                return RedirectToLocal(returnUrl);
            }
            AddErrors(result);
        }

        // If we got this far, something failed, redisplay form
        return View(viewModel);
    }

    //
    // POST: /Account/LogOut
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await _signInManagerService.SignOutAsync();
        _logger.LogInformation(4, "User logged out.");
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    //
    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Action(
            "ExternalLoginCallback",
            "Account",
            new { ReturnUrl = returnUrl }
        );
        var properties = _signInManagerService.ConfigureExternalAuthenticationProperties(
            provider,
            redirectUrl
        );
        return Challenge(properties, provider);
    }

    //
    // GET: /Account/ExternalLoginCallback
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(
        string? returnUrl = null,
        string? remoteError = null
    )
    {
        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
            return View(nameof(Login));
        }

        var info = await _signInManagerService.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction(nameof(Login));
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManagerService.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false
        );
        if (result.Succeeded)
        {
            // Update any authentication tokens if login succeeded
            await _signInManagerService.UpdateExternalAuthenticationTokensAsync(info);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    5,
                    "User logged in with {Name} provider.",
                    info.LoginProvider
                );
            }

            return RedirectToLocal(returnUrl);
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
        }

        if (result.IsLockedOut)
        {
            return View("Lockout");
        }

        // If the user does not have an account, then ask the user to create an account.
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
            return View("Error");

        return View(
            "ExternalLoginConfirmation",
            new ExternalLoginConfirmationViewModel { Email = email }
        );
    }

    //
    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(
        ExternalLoginConfirmationViewModel viewModel,
        string? returnUrl = null
    )
    {
        if (ModelState.IsValid)
        {
            // Get the information about the user from the external login provider
            var info = await _signInManagerService.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View("ExternalLoginFailure");
            }
            var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
            var result = await _userManagerService.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManagerService.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManagerService.SignInAsync(user, isPersistent: false);
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                            6,
                            "User created an account using {Name} provider.",
                            info.LoginProvider
                        );
                    }

                    // Update any authentication tokens as well
                    await _signInManagerService.UpdateExternalAuthenticationTokensAsync(info);

                    return RedirectToLocal(returnUrl);
                }
            }
            AddErrors(result);
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(viewModel);
    }

    // GET: /Account/ConfirmEmail
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error");
        }
        var user = await _userManagerService.FindByIdAsync(userId);
        if (user == null)
        {
            return View("Error");
        }
        var result = await _userManagerService.ConfirmEmailAsync(user, code);
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    //
    // GET: /Account/ForgotPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    //
    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManagerService.FindByEmailAsync(viewModel.Email);
            if (user == null || !(await _userManagerService.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
            // Send an email with this link
            //var code = await _userManagerService.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            //await _emailSender.SendEmailAsync(viewModel.Email, "Reset Password",
            //   "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            //return View("ForgotPasswordConfirmation");
        }

        // If we got this far, something failed, redisplay form
        return View(viewModel);
    }

    //
    // GET: /Account/ForgotPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    //
    // GET: /Account/ResetPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string? code = null)
    {
        return code == null ? View("Error") : View();
    }

    //
    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        var user = await _userManagerService.FindByEmailAsync(viewModel.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        }
        var result = await _userManagerService.ResetPasswordAsync(
            user,
            viewModel.Code,
            viewModel.Password
        );
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        }
        AddErrors(result);
        return View();
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    //
    // GET: /Account/SendCode
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> SendCode(string? returnUrl = null, bool rememberMe = false)
    {
        var user = await _signInManagerService.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return View("Error");
        }
        var userFactors = await _userManagerService.GetValidTwoFactorProvidersAsync(user);
        var factorOptions = userFactors
            .Select(purpose => new SelectListItem { Text = purpose, Value = purpose })
            .ToList();
        return View(
            new SendCodeViewModel
            {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe,
            }
        );
    }

    //
    // POST: /Account/SendCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendCode(SendCodeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _signInManagerService.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return View("Error");
        }

        if (viewModel.SelectedProvider == "Authenticator")
        {
            return RedirectToAction(
                nameof(VerifyAuthenticatorCode),
                new { viewModel.ReturnUrl, viewModel.RememberMe }
            );
        }

        // Generate the token and send it
        var code = await _userManagerService.GenerateTwoFactorTokenAsync(
            user,
            viewModel.SelectedProvider!
        );
        if (string.IsNullOrWhiteSpace(code))
        {
            return View("Error");
        }

        var message = "Your security code is: " + code;
        if (viewModel.SelectedProvider == "Email")
        {
            var email = await _userManagerService.GetEmailAsync(user);
            if (string.IsNullOrWhiteSpace(email))
            {
                return View("Error");
            }

            await _emailSender.SendEmailAsync(email, "Security Code", message);
        }
        else if (viewModel.SelectedProvider == "Phone")
        {
            var phoneNumber = await _userManagerService.GetPhoneNumberAsync(user);
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return View("Error");
            }

            await _smsSender.SendSmsAsync(phoneNumber, message);
        }

        return RedirectToAction(
            nameof(VerifyCode),
            new
            {
                Provider = viewModel.SelectedProvider,
                viewModel.ReturnUrl,
                viewModel.RememberMe,
            }
        );
    }

    //
    // GET: /Account/VerifyCode
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode(
        string provider,
        bool rememberMe,
        string? returnUrl = null
    )
    {
        // Require that the user has already logged in via username/password or external login
        var user = await _signInManagerService.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return View("Error");
        }
        return View(
            new VerifyCodeViewModel
            {
                Provider = provider,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe,
            }
        );
    }

    //
    // POST: /Account/VerifyCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyCode(VerifyCodeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        // The following code protects for brute force attacks against the two factor codes.
        // If a user enters incorrect codes for a specified amount of time then the user account
        // will be locked out for a specified amount of time.
        var result = await _signInManagerService.TwoFactorSignInAsync(
            viewModel.Provider,
            viewModel.Code,
            viewModel.RememberMe,
            viewModel.RememberBrowser
        );
        if (result.Succeeded)
        {
            return RedirectToLocal(viewModel.ReturnUrl);
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning(7, "User account locked out.");
            return View("Lockout");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid code.");
            return View(viewModel);
        }
    }

    //
    // GET: /Account/VerifyAuthenticatorCode
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyAuthenticatorCode(
        bool rememberMe,
        string? returnUrl = null
    )
    {
        // Require that the user has already logged in via username/password or external login
        var user = await _signInManagerService.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return View("Error");
        }
        return View(
            new VerifyAuthenticatorCodeViewModel { ReturnUrl = returnUrl, RememberMe = rememberMe }
        );
    }

    //
    // POST: /Account/VerifyAuthenticatorCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyAuthenticatorCode(
        VerifyAuthenticatorCodeViewModel viewModel
    )
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        // The following code protects for brute force attacks against the two factor codes.
        // If a user enters incorrect codes for a specified amount of time then the user account
        // will be locked out for a specified amount of time.
        var result = await _signInManagerService.TwoFactorAuthenticatorSignInAsync(
            viewModel.Code,
            viewModel.RememberMe,
            viewModel.RememberBrowser
        );
        if (result.Succeeded)
        {
            return RedirectToLocal(viewModel.ReturnUrl);
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning(7, "User account locked out.");
            return View("Lockout");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid code.");
            return View(viewModel);
        }
    }

    //
    // GET: /Account/UseRecoveryCode
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> UseRecoveryCode(string? returnUrl = null)
    {
        // Require that the user has already logged in via username/password or external login
        var user = await _signInManagerService.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return View("Error");
        }
        return View(new UseRecoveryCodeViewModel { ReturnUrl = returnUrl });
    }

    //
    // POST: /Account/UseRecoveryCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UseRecoveryCode(UseRecoveryCodeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if (string.IsNullOrWhiteSpace(viewModel.Code))
        {
            ModelState.AddModelError(nameof(viewModel.Code), "Recovery code is required.");
            return View(viewModel);
        }

        var result = await _signInManagerService.TwoFactorRecoveryCodeSignInAsync(viewModel.Code);
        if (result.Succeeded)
        {
            return RedirectToLocal(viewModel.ReturnUrl);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid code.");
            return View(viewModel);
        }
    }

    #region Helpers

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    private async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var user = await _userManagerService.GetUserAsync(HttpContext.User);
        return user ?? throw new InvalidOperationException("No authenticated user found.");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    #endregion
}
