using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AccountManager.Domain.Validators;
using AccountManager.Infrastructure.Models;
using AccountManager.Infrastructure.Services;

namespace AccountManager.Infrastructure.Configurations
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Validator<>).Assembly)
                .AsClosedTypesOf(typeof(Validator<>));

            builder.RegisterAssemblyTypes(typeof(AccountService).Assembly)
                .AsImplementedInterfaces();

            RegisterDbContext(builder);
        }

        private void RegisterDbContext(ContainerBuilder builder)
        {
            // This is necessary for the integration tests to run correctly.
            // Will use the same instance of the in-memory DB for every test run,
            // BUT each separate test run will have a different in-memory DB
            var inMemId = Guid.NewGuid().ToString();

            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var connString = config.GetConnectionString("AccountManagerDbConnectionString");
                var opt = new DbContextOptionsBuilder<AccountManagerDbContext>();
                opt.UseSqlServer(connString);

                return new AccountManagerDbContext(opt.Options);

            }).InstancePerLifetimeScope();
        }
    }
}
