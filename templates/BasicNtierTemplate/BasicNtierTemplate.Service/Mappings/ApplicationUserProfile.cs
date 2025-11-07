using AutoMapper;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Mappings
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            // Entity -> DTO
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();

            // DTO -> Entity
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();
        }
    }
}
