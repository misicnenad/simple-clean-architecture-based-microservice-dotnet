using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace UserManager.Domain
{
    public abstract class BaseRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IPreProcessHandler<TRequest, TResponse>> _preProcessHandlers;
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;

        protected BaseRequestHandler(
            IEnumerable<IPreProcessHandler<TRequest, TResponse>> preProcessHandlers, 
            IRequestHandler<TRequest, TResponse> requestHandler)
        {
            _preProcessHandlers = preProcessHandlers;
            _requestHandler = requestHandler;
        }

        public Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default)
        {
            Task<TResponse> handler() => _requestHandler.HandleAsync(request, ct);

            return _preProcessHandlers
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)handler,
                    (next, pipeline) => () => pipeline.HandleAsync(request, next, ct)
                )();
        }
    }
}
