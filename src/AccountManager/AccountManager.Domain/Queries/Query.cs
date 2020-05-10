using System;
using MediatR;

namespace AccountManager.Domain.Queries
{
    public abstract class Query<TResult> : IRequest<TResult>
    {
        protected Query()
        {
            QueryId = Guid.NewGuid();
            CorrelationId = QueryId;
        }

        protected Query(Guid correlationId)
        {
            QueryId = Guid.NewGuid();
            CorrelationId = correlationId;
        }

        public Guid QueryId { get; }
        public Guid CorrelationId { get; }
    }

    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : Query<TResult>
    {
    }
}
