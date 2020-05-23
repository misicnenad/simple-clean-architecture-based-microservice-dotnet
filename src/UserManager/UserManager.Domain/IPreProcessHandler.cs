using System.Threading.Tasks;
using System.Threading;

namespace UserManager.Domain
{
    public interface IPreProcessHandler<TRequest> where TRequest : IRequest
    {
        Task HandleAsync(TRequest request, RequestHandlerDelegate next, CancellationToken ct = default);
    }

    public interface IPreProcessHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct = default);
    }
}
