using AutoMapper;
using BasicNtierTemplate.Service.Dtos.User;
using BasicNtierTemplate.Web.MVC.Models.ViewModels;

namespace BasicNtierTemplate.Web.MVC.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUserDto, UserViewModel>().ReverseMap();
        }
    }
}
