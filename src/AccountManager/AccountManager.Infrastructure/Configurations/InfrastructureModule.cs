using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Validators;
using AccountManager.Infrastructure.Services;

using Autofac;

namespace AccountManager.Infrastructure.Configurations
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatRModule>();

            builder.RegisterAssemblyTypes(typeof(Validator<>).Assembly)
                .AsClosedTypesOf(typeof(Validator<>));

            builder.RegisterType<AccountService>().As<IAccountService>();
        }
    }
}
