using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Domain.Providers;

namespace AccountManager.Domain.Commands
{
    public class CreateAccountHandler : ICommandHandler<CreateAccount, Account>
    {
        private readonly IAccountService _accountService;
        private readonly DateTimeProvider _dateTimeProvider;

        public CreateAccountHandler(
            IAccountService accountService, 
            DateTimeProvider dateTimeProvider)
        {
            _accountService = accountService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Account> Handle(CreateAccount request, CancellationToken cancellationToken = default)
        {
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
    public class CreateAccount : Command<Account>
    {
        public CreateAccount(int userId, AccountType accountType)
        {
            UserId = userId;
            AccountType = accountType;
        }

        public int UserId { get; }
        public AccountType AccountType { get; }
    }
}
