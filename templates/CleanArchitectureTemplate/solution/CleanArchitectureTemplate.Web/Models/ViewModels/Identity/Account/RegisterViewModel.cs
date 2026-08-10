// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureTemplate.Web.Models.ViewModels.Identity.Account;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "First name")]
    public string? FirstName { get; set; }

    [Required]
    [Display(Name = "Last name")]
    public string? LastName { get; set; }

    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = default!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required]
    [StringLength(
        100,
        MinimumLength = 6,
        ErrorMessage = "The {0} must be at least {2} characters long."
    )]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(
        nameof(Password),
        ErrorMessage = "The password and confirmation password do not match."
    )]
    public string? ConfirmPassword { get; set; }
}
