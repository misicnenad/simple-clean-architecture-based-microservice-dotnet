using AccountManager.Domain.Models;
using AccountManager.Infrastructure.Models;
using AutoMapper;

namespace AccountManager.Infrastructure.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDbo>()
                .ReverseMap();
        }
    }
}
