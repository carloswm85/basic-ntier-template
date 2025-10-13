using AutoMapper;
using BasicNtierTemplate.Data.Entities;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Mappings
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogDto>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.id))
                .ForMember(dto => dto.Url, opt => opt.MapFrom(src => src.url))
                .ForMember(dto => dto.Posts, opt => opt.MapFrom(src => src.posteos))
                ;

            CreateMap<BlogDto, Blog>()
                .ForMember(destination => destination.id, options => options.MapFrom(source => source.Id))
                .ForMember(dto => dto.url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dto => dto.posteos, opt => opt.MapFrom(src => src.Posts))
                ;
        }
    }
}
