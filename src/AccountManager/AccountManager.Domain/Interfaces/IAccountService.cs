using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Models;

namespace AccountManager.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<Account> AddAsync(Guid correlationId, Account account, CancellationToken ct = default);
    }
}
