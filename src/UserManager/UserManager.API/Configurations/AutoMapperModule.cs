using System.Reflection;
using Autofac;
using AutoMapper;
using UserManager.API.Models;
using UserManager.Domain.Models;

namespace UserManager.API.Configurations
{
    public class AutoMapperModule : Autofac.Module
    {
        private readonly Assembly[] _profileAssemblies;

        public AutoMapperModule(params Assembly[] profileAssemblies)
        {
            _profileAssemblies = profileAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(_profileAssemblies);

                cfg.CreateMap<User, UserDto>();
            });

            builder.RegisterInstance(config).As<IConfigurationProvider>().ExternallyOwned();
            builder.RegisterType<Mapper>().As<IMapper>();
        }
    }
}
