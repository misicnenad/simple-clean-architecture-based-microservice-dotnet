using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using AccountManager.Domain.Commands;
using Shared.Messages;

namespace AccountManager.Worker.Configurations
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

                cfg.CreateMap<CreateAccountMessage, CreateAccount>();
            });

            builder.RegisterInstance(config).As<IConfigurationProvider>().ExternallyOwned();
            builder.RegisterType<Mapper>().As<IMapper>();
        }
    }
}
