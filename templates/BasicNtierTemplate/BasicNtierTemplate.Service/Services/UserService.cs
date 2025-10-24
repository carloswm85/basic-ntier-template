using AutoMapper;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Services.Interfaces;

namespace BasicNtierTemplate.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
