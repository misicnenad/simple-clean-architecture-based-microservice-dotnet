using System.Threading.Tasks;
using System.Threading;

namespace UserManager.Domain
{
    public interface IPreProcessHandler<TRequest> : IPreProcessHandler<TRequest, Void> where TRequest : IRequest
    {
    }

    public interface IPreProcessHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct = default);
    }
}
