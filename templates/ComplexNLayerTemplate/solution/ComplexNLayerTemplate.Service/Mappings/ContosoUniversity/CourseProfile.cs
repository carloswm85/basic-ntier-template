using AutoMapper;
using ComplexNLayerTemplate.Data.Model;
using ComplexNLayerTemplate.Service.Dtos.ContosoUniversity;

namespace ComplexNLayerTemplate.Service.Mappings.ContosoUniversity
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