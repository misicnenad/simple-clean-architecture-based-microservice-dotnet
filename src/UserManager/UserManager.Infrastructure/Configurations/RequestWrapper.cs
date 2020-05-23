using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace UserManager.Infrastructure.Configurations
{
    public class RequestHandlerWrapper<TRequest, TType, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : RequestWrapper<TType, TResponse>
        where TType : Domain.IRequest<TResponse>
    {
        private readonly Domain.IRequestHandler<TType, TResponse> _requestHandler;

        public RequestHandlerWrapper(Domain.IRequestHandler<TType, TResponse> requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
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
