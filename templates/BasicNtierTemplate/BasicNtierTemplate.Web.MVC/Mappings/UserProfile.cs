using AutoMapper;
using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Web.MVC.Models.ViewModels;

namespace BasicNtierTemplate.Web.MVC.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUserDto, UserViewModel>().ReverseMap();
            CreateMap<UserViewModel, CreateUserCommand>().ReverseMap();
        }
    }
}
