using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Infrastructure.Models;
using AutoMapper;

namespace AccountManager.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILoggerService<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly AccountManagerDbContext _dbContext;

        public AccountService(
            ILoggerService<AccountService> logger,
            IMapper mapper,
            AccountManagerDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Account> AddAsync(Guid correlationId, Account account, CancellationToken ct = default)
        {
            _logger.LogInfo(correlationId, $"Executing {nameof(AccountService)}.{nameof(AccountService.AddAsync)}");

            var dboToAdd = _mapper.Map<AccountDbo>(account);

            _logger.LogInfo(correlationId, $"Saving {nameof(Account)} for {nameof(Account.UserId)}={account.UserId}");

            var newAccountDbo = await _dbContext.AddAsync(dboToAdd, ct);
            await _dbContext.SaveChangesAsync(ct);

            var entity = newAccountDbo.Entity;

            _logger.LogInfo(correlationId, $"{nameof(Account)} saved with {nameof(Account.AccountId)}={entity.AccountId}");

            return _mapper.Map<Account>(entity);
        }
    }
}
