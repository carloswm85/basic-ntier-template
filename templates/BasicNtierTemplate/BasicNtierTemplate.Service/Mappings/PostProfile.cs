using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            // Entity -> DTO
            CreateMap<Posteo, PostDto>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(src => src.titulo))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(src => src.contenido))
                .ForMember(dto => dto.BlogId, opt => opt.MapFrom(src => src.blogid))
                .ForMember(dto => dto.Blog, opt => opt.Ignore())
                ;

            // DTO -> Entity
            CreateMap<PostDto, Posteo>()
                .ForMember(destination => destination.id, options => options.MapFrom(source => source.Id))
                .ForMember(dto => dto.titulo, opt => opt.MapFrom(src => src.Title))
                .ForMember(dto => dto.contenido, opt => opt.MapFrom(src => src.Content))
                .ForMember(dto => dto.blogid, opt => opt.MapFrom(src => src.BlogId))
                .ForMember(dto => dto.blog, opt => opt.Ignore())
                ;
        }
    }
}
