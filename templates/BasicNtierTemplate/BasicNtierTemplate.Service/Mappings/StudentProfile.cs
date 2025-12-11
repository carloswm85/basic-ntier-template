using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos.Contoso;

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

            // Student -> StudentDto
            CreateMap<Student, StudentDto>()
                .ForMember(destination => destination.FullName, options => options.Ignore())
                .ForMember(dest => dest.GovernmentIdFormatted, opt => opt.Ignore())
                .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
            ;

            // StudentDto -> Student (explicit control)
            CreateMap<StudentDto, Student>()
                .ForMember(destination => destination.FullName, options => options.Ignore())
                .ForMember(dest => dest.Enrollments, opt => opt.Ignore()) // Handle separately
            ;

        }
    }
}
