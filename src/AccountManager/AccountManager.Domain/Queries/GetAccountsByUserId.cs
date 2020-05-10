using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;

namespace AccountManager.Domain.Queries
{
    public class GetAccountsByUserIdHandler : IQueryHandler<GetAccountsByUserId, IEnumerable<Account>>
    {
        private readonly IAccountService _accountService;

        public GetAccountsByUserIdHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IEnumerable<Account>> Handle(GetAccountsByUserId request, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountService.GetAllByUserIdAsync(request.CorrelationId, request.UserId, cancellationToken);

            return accounts;
        }
    }

    public class GetAccountsByUserId : Query<IEnumerable<Account>>
    {
        public GetAccountsByUserId(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}
