using AccountManager.Domain.Commands;
using FluentValidation;

namespace AccountManager.Domain.Validators
{
    public class CreateAccountValidator : Validator<CreateAccount>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.CommandId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.AccountType).NotEmpty();
        }
    }
}
