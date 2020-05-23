using UserManager.Domain.Commands;
using UserManager.Domain.Queries;
using FluentValidation;

namespace UserManager.Domain.Validators
{
    public abstract class QueryValidator<TQuery, TQueryResult> 
        : AbstractValidator<TQuery> where TQuery : Query<TQueryResult>
    {
        protected QueryValidator()
        {
            RuleFor(x => x.QueryId).NotEmpty();
            RuleFor(x => x.CorrelationId).NotEmpty();
        }
    }

    public abstract class CommandValidator<T> : AbstractValidator<T> where T : BaseCommand
    {
        protected CommandValidator()
        {
            RuleFor(x => x.CommandId).NotEmpty();
            RuleFor(x => x.CorrelationId).NotEmpty();
        }
    }
}
