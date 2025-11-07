using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Web.MVC.Models.ViewModels;

namespace BasicNtierTemplate.Web.MVC.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        // Optional, depending on your app
        public IList<string> Roles { get; set; } = new List<string>();
        public IList<string> Claims { get; set; } = new List<string>();
    }
}

public static class UserExtensions
{
    public static UserViewModel ToViewModel(this ApplicationUserDto user, IList<string> roles, IList<string> claims)
    {
        return new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnd = user.LockoutEnd,
            LockoutEnabled = user.LockoutEnabled,
            AccessFailedCount = user.AccessFailedCount,
            Roles = roles,
            Claims = claims
        };
    }
}
