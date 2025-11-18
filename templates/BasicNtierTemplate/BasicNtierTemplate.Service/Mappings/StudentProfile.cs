using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            //CreateMap<ExampletDto, Example>()
            //    .ForMember(destination => destination.id, options => options.MapFrom(source => source.Id))
            //    .ForMember(dto => dto.some_field, opt => opt.MapFrom(src => src.SomeFiled))
            //    ;

            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
