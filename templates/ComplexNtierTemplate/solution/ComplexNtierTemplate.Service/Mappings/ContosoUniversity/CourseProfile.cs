using AutoMapper;
using ComplexNtierTemplate.Data.Model;
using ComplexNtierTemplate.Service.Dtos.ContosoUniversity;

namespace ComplexNtierTemplate.Service.Mappings.ContosoUniversity
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