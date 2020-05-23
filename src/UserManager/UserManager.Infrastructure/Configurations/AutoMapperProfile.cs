using AutoMapper;
using UserManager.Domain.Models;
using UserManager.Infrastructure.Models;

namespace UserManager.Infrastructure.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDbo, User>();
            CreateMap<AccountDbo, Account>();
        }
    }
}
