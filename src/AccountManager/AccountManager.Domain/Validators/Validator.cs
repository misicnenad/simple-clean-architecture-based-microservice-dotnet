using AccountManager.Domain.Commands;
using AccountManager.Domain.Queries;
using FluentValidation;

namespace AccountManager.Domain.Validators
{
    // Abstract class containing methods used for mocking in tests (extension methods can't be mocked)
    public abstract class Validator<T> : AbstractValidator<T>
    {
        public virtual void ValidateAndThrow(T instance)
        {
            this.ValidateAndThrow(instance, null);
        }
    }

    public abstract class QueryValidator<TQuery, TQueryResult> 
        : Validator<TQuery> where TQuery : Query<TQueryResult>
    {
        protected QueryValidator()
        {
            RuleFor(x => x.QueryId).NotEmpty();
            RuleFor(x => x.CorrelationId).NotEmpty();
        }
    }

    public abstract class CommandValidator<T> : Validator<T> where T : BaseCommand
    {
        protected CommandValidator()
        {
            RuleFor(x => x.CommandId).NotEmpty();
            RuleFor(x => x.CorrelationId).NotEmpty();
        }
    }
}
