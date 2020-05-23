using System.Collections.Generic;
using UserManager.Domain.Queries;

namespace UserManager.Domain
{
    public abstract class QueryHandler<TQuery, TResult>
        : BaseRequestHandler<TQuery, TResult> where TQuery : Query<TResult>
    {
        protected QueryHandler(IEnumerable<IPreProcessHandler<TQuery, TResult>> preProcessHandlers) 
            : base(preProcessHandlers)
        {
        }
    }
}
