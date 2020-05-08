using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Domain.Providers;
using AccountManager.Domain.Validators;

namespace AccountManager.Domain.Commands
{
    public class CreateAccountHandler : ICommandHandler<CreateAccount, Account>
    {
        private readonly IAccountService _accountService;
        private readonly Validator<CreateAccount> _validationRules;
        private readonly DateTimeProvider _dateTimeProvider;

        public CreateAccountHandler(
            IAccountService accountService, 
            Validator<CreateAccount> validationRules, 
            DateTimeProvider dateTimeProvider)
        {
            _accountService = accountService;
            _validationRules = validationRules;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Account> Handle(CreateAccount request, CancellationToken cancellationToken = default)
        {
            _validationRules.ValidateAndThrow(request);

            var accountToAdd = new Account
            {
                UserId = request.UserId,
                AccountType = request.AccountType,
                DateCreated = _dateTimeProvider.UtcNow
            };
            var newAccount = await _accountService.AddAsync(request.CorrelationId, accountToAdd, cancellationToken);

            return newAccount;
        }
    }
    public class CreateAccount : ICommand<Account>
    {
        public CreateAccount(int userId, AccountType accountType)
        {
            CommandId = Guid.NewGuid();
            CorrelationId = CommandId;
            UserId = userId;
            AccountType = accountType;
        }

        public Guid CommandId { get; }
        public Guid CorrelationId { get; }
        public int UserId { get; }
        public AccountType AccountType { get; }
    }
}
