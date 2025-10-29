using Microsoft.AspNetCore.Identity;

namespace BasicNtierTemplate.Data.Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? City { get; set; }
    }
}
