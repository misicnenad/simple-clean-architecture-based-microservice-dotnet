using UserManager.Domain;
using UserManager.Domain.Validators;

using Autofac;

using FluentValidation;
using UserManager.Infrastructure.Services;
using UserManager.Domain.Interfaces;
using UserManager.Infrastructure.Providers;

namespace UserManager.Infrastructure.Configurations
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatRModule>();

            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterAssemblyTypes(typeof(CommandValidator<>).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>));

            builder.RegisterGeneric(typeof(ValidationBehavior<>))
                  .As(typeof(IPreProcessHandler<>));

            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>().SingleInstance();
        }
    }
}
