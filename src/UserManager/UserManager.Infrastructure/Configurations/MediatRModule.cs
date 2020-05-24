using Autofac;
using MediatR;

namespace UserManager.Infrastructure.Configurations
{
    internal class MediatRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();

            var domainRequestHandlerType = typeof(Domain.IRequestHandler<,>);
            
            builder.RegisterRequestHandlers(domainRequestHandlerType.Assembly);

            builder.RegisterAssemblyTypes(domainRequestHandlerType.Assembly)
                .AsClosedTypesOf(domainRequestHandlerType);

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}
