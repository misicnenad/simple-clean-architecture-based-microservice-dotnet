using System;

namespace UserManager.Domain
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
}
