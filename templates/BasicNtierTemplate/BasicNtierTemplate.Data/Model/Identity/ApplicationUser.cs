using Microsoft.AspNetCore.Identity;

namespace BasicNtierTemplate.Data.Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string? FirstName { get; set; }
        public required string? LastName { get; set; }
        public required string? City { get; set; }

    }
}

// Inherited properties from IdentityUser:
/* ✔️: Properties in ApplicationUserDto
- Id ✔️
- UserName ✔️
- NormalizedUserName
- Email ✔️
- NormalizedEmail
- EmailConfirmed ✔️
- PasswordHash
- SecurityStamp
- ConcurrencyStamp
- PhoneNumber ✔️
- PhoneNumberConfirmed ✔️
- TwoFactorEnabled ✔️
- LockoutEnd
- LockoutEnabled ✔️
- AccessFailedCount ✔️
// Also:
- Roles
- Claims
*/