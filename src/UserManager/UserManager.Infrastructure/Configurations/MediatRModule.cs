using System.Threading;
using System.Threading.Tasks;
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

            builder.RegisterAssemblyTypes(typeof(Request<,>).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }

    public class Request<T, R> : IRequest<R>, UserManager.Domain.IRequest<R>
        where T : Domain.IRequest<R>
    {

    }

    public class RequestHandler<T, R> : IRequestHandler<T, R> 
        where T : Request<T, R>
    {
        private readonly Domain.IRequestHandler<T, R> _requestHandler;

        public RequestHandler(Domain.IRequestHandler<T, R> requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public Task<R> Handle(T request, CancellationToken cancellationToken = default)
        {
            return _requestHandler.HandleAsync(request, cancellationToken);
        }
    }
}
