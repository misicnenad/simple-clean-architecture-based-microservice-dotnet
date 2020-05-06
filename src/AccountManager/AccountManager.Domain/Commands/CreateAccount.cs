using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Domain.Providers;
using AccountManager.Domain.Validators;

namespace AccountManager.Domain.Commands
{
    public class CreateAccountHandler : ICommandHandler<CreateAccount, int>
    {
        private readonly IAccountService _accountService;
        private readonly Validator<CreateAccount> _validator;
        private readonly DateTimeProvider _dateTimeProvider;

        public CreateAccountHandler(
            IAccountService accountService, 
            Validator<CreateAccount> validator, 
            DateTimeProvider dateTimeProvider)
        {
            _accountService = accountService;
            _validator = validator;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> Handle(CreateAccount request, CancellationToken cancellationToken = default)
        {
            _validator.ValidateAndThrow(request);

            var accountToAdd = new Account
            {
                UserId = request.UserId,
                AccountType = request.AccountType,
                DateCreated = _dateTimeProvider.UtcNow
            };
            var newAccount = await _accountService.AddAsync(accountToAdd, cancellationToken);

            return newAccount.AccountId;
        }
    }
    public class CreateAccount : ICommand<int>
    {
        public CreateAccount(int userId, AccountType accountType)
        {
            CommandId = Guid.NewGuid();
            UserId = userId;
            AccountType = accountType;
        }

        public Guid CommandId { get; }
        public int UserId { get; }
        public AccountType AccountType { get; }
    }
}
