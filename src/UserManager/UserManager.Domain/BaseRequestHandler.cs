using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace UserManager.Domain
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IPreProcessHandler<TRequest, TResponse>> _preProcessHandlers;

        protected BaseRequestHandler(IEnumerable<IPreProcessHandler<TRequest, TResponse>> preProcessHandlers)
        {
            _preProcessHandlers = preProcessHandlers;
        }

        public Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default)
        {
            Task<TResponse> handler() => HandlerInternalAsync(request, ct);

            return _preProcessHandlers
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)handler,
                    (next, pipeline) => () => pipeline.HandleAsync(request, next, ct)
                )();
        }

        protected abstract Task<TResponse> HandlerInternalAsync(TRequest request, CancellationToken ct = default);
    }
}
