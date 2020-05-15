using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Providers;
using AccountManager.Domain.Validators;
using AccountManager.Infrastructure.Services;

using Autofac;

using FluentValidation;

using MediatR;

namespace AccountManager.Infrastructure.Configurations
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatRModule>();

            builder.RegisterAssemblyTypes(typeof(CommandValidator<>).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>));

            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
                  .As(typeof(IPipelineBehavior<,>));

            builder.RegisterType<AccountService>().As<IAccountService>();

            builder.RegisterType<DateTimeProvider>().SingleInstance();
        }
    }
}
