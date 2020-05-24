namespace UserManager.Domain
{
    public interface IQueryHandler<TQuery, TResult>
        : IRequestHandler<TQuery, TResult> where TQuery : Query<TResult>
    {
    }
}
