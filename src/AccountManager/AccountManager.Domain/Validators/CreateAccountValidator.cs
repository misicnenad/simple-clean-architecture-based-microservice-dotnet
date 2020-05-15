using AccountManager.Domain.Commands;
using FluentValidation;

namespace AccountManager.Domain.Validators
{
    public class CreateAccountValidator : CommandValidator<CreateAccount>
    {
        public CreateAccountValidator()
        {
            const int minimumValidUserId = 1;
            RuleFor(x => x.UserId).GreaterThanOrEqualTo(minimumValidUserId);
            RuleFor(x => x.AccountType).NotEmpty();
        }
    }
}
