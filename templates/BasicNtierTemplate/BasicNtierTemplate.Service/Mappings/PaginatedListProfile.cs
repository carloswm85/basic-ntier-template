using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Mappings
{
    public class PaginatedListProfile : Profile
    {
        public PaginatedListProfile()
        {
            CreateMap<PaginatedList<Student>, PaginatedList<StudentDto>>();
        }
    }
}
