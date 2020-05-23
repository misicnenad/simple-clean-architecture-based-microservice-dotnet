using Autofac;
using MediatR;
using UserManager.Domain.Commands;

namespace UserManager.Infrastructure.Configurations
{
    internal class MediatRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterRequestHandlers();

            builder.RegisterAssemblyTypes(typeof(Command).Assembly)
                .AsClosedTypesOf(typeof(Domain.IRequestHandler<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}
