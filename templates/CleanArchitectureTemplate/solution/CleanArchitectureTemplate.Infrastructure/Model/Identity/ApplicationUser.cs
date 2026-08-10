using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Infrastructure.Model.Identity;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = true;
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? ImagePath { get; set; }
}
