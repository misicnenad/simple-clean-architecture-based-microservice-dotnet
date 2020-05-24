using System.Threading.Tasks;
using System.Threading;
using System;

namespace UserManager.Domain
{
    public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Void> where TRequest : IRequest
    {
    }

    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
    }

    public interface IRequest : IRequest<Void>
    {
    }

    public interface IRequest<out TResponse>
    {
        public Guid CorrelationId { get; }
    }

    public sealed class Void 
    {
        public static Void Value => new Void();
    }
}
