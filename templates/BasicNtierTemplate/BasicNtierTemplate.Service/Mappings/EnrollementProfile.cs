using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Mappings
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
        }
    }
}
