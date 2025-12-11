using AutoMapper;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Service.Dtos.User;

namespace BasicNtierTemplate.Service.Mappings
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();

            CreateMap<ApplicationUser, ListedApplicationUserDto>();
        }
    }
}
