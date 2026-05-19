using AutoMapper;
using SimpleMonolithTemplate.Module.Data.Entities.ContosoUniversity;
using SimpleMonolithTemplate.Module.Dtos.ContosoUniversity;

namespace SimpleMonolithTemplate.Module.Mappings.ContosoUniversity;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        //CreateMap<ExampletDto, Example>()
        //    .ForMember(destination => destination.id, options => options.MapFrom(source => source.Id))
        //    .ForMember(dto => dto.some_field, opt => opt.MapFrom(src => src.SomeFiled))
        //    ;

        CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
    }
}
