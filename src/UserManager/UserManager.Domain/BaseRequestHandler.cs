using System.Threading.Tasks;
using System.Threading;

namespace UserManager.Domain
{
    public abstract class BaseRequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly IPreProcessHandler<TRequest> _preProcessHandler;

        protected BaseRequestHandler(IPreProcessHandler<TRequest> preProcessHandler)
        {
            _preProcessHandler = preProcessHandler;
        }

        public async Task HandleAsync(TRequest request, CancellationToken ct = default)
        {
            await _preProcessHandler.HandleAsync(request, HandlerInternalAsync, ct);
        }

        protected abstract Task HandlerInternalAsync(TRequest request, CancellationToken ct = default);
    }

    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IPreProcessHandler<TRequest, TResponse> _preProcessHandler;

        protected BaseRequestHandler(IPreProcessHandler<TRequest, TResponse> preProcessHandler)
        {
            _preProcessHandler = preProcessHandler;
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default)
        {
            return await _preProcessHandler.HandleAsync(request, HandlerInternalAsync, ct);
        }

        protected abstract Task<TResponse> HandlerInternalAsync(TRequest request, CancellationToken ct);
    }
}
