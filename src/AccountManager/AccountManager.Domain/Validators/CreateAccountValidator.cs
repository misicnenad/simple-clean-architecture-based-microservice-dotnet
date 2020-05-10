using AccountManager.Domain.Commands;
using FluentValidation;

namespace AccountManager.Domain.Validators
{
    public class CreateAccountValidator : CommandValidator<CreateAccount>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.AccountType).NotEmpty();
        }
    }
}
