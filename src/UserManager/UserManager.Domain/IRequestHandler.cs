using System.Threading.Tasks;
using System.Threading;

namespace UserManager.Domain
{
    public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Void> where TRequest : IRequest
    {
    }

    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
    }
}
