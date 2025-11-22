using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;


// TODO delete this class?
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
