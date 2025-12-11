using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos.Contoso;

namespace BasicNtierTemplate.Service.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            //CreateMap<ExampletDto, Example>()
            //    .ForMember(destination => destination.id, options => options.MapFrom(source => source.Id))
            //    .ForMember(dto => dto.some_field, opt => opt.MapFrom(src => src.SomeFiled))
            //    ;

            CreateMap<Course, CourseDto>().ReverseMap();
        }
    }
}