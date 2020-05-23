using System.Threading.Tasks;
using System.Threading;
using System;

namespace UserManager.Domain
{
    public interface IPreProcessHandler<TRequest> where TRequest : IRequest
    {
        Task HandleAsync(TRequest request, Func<TRequest, CancellationToken, Task> next, CancellationToken ct = default);
    }

    public interface IPreProcessHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, Func<TRequest, CancellationToken, Task<TResponse>> next, CancellationToken ct = default);
    }
}
