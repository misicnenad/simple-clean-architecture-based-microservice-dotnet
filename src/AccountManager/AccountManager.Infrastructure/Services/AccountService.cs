using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Infrastructure.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

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

        public async Task<Account> AddAsync(Guid correlationId, Account account, CancellationToken ct = default)
        {
            var dboToAdd = _mapper.Map<AccountDbo>(account);

            var newAccountDbo = await _dbContext.AddAsync(dboToAdd, ct);
            await _dbContext.SaveChangesAsync(ct);

            var entity = newAccountDbo.Entity;

            return _mapper.Map<Account>(entity);
        }

        public async Task<IEnumerable<Account>> GetAllByUserIdAsync(Guid correlationId, int userId, CancellationToken ct = default)
        {
            var accounts = await _dbContext.Accounts
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<Account>>(accounts);
        }
    }
}
