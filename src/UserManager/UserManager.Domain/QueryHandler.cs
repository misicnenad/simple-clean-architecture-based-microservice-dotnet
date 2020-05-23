using UserManager.Domain.Queries;

namespace UserManager.Domain
{
    public abstract class QueryHandler<TQuery, TResult>
        : BaseRequestHandler<TQuery, TResult> where TQuery : Query<TResult>
    {
        protected QueryHandler(IPreProcessHandler<TQuery, TResult> preProcessHandler)
            : base(preProcessHandler)
        {
        }
    }
}
