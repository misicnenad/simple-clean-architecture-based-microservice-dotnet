﻿using System.Reflection;
using AccountManager.API.Models;
using AccountManager.Domain.Models;
using Autofac;
using AutoMapper;

namespace AccountManager.API.Configurations
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

                cfg.CreateMap<Account, AccountDto>();
            });

            builder.RegisterInstance(config).As<IConfigurationProvider>().ExternallyOwned();
            builder.RegisterType<Mapper>().As<IMapper>();
        }
    }
}
