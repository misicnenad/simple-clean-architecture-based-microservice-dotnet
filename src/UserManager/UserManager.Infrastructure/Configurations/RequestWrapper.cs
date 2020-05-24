using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace UserManager.Infrastructure.Configurations
{
    public class RequestHandlerWrapper<TRequestWrapper, TRequest, TResponse>: IRequestHandler<TRequestWrapper, TResponse>
        where TRequestWrapper : RequestWrapper<TRequest, TResponse>
        where TRequest : Domain.IRequest<TResponse>
    {
        private readonly Domain.IRequestHandler<TRequest, TResponse> _requestHandler;

        public RequestHandlerWrapper(Domain.IRequestHandler<TRequest, TResponse> requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public Task<TResponse> Handle(TRequestWrapper request, CancellationToken cancellationToken = default)
        {
            return _requestHandler.HandleAsync(request.Request, cancellationToken);
        }
    }

    public class RequestWrapper<TRequest, TResponse> : IRequest<TResponse>
        where TRequest : Domain.IRequest<TResponse>
    {
        public RequestWrapper(TRequest request)
        {
            Request = request;
        }

        public TRequest Request { get; }
    }
}
