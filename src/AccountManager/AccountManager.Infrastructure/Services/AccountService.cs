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
        private readonly IMapper _mapper;
        private readonly AccountManagerDbContext _dbContext;

        public AccountService(IMapper mapper, AccountManagerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Account> AddAsync(Account account, CancellationToken ct = default)
        {
            var dboToAdd = _mapper.Map<AccountDbo>(account);
            
            var newAccountDbo = await _dbContext.AddAsync(dboToAdd, ct);
            await _dbContext.SaveChangesAsync(ct);
            
            return _mapper.Map<Account>(newAccountDbo.Entity);
        }
    }
}
