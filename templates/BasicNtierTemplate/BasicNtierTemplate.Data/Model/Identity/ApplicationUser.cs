using Microsoft.AspNetCore.Identity;

namespace BasicNtierTemplate.Data.Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string City { get; set; }

    }
}
