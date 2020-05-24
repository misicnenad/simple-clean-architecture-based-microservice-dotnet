using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Models;

namespace UserManager.Domain.Commands
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly IUserService _userService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateUserHandler(IUserService userService, IDateTimeProvider dateTimeProvider)
        {
            _userService = userService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Void> HandleAsync(CreateUser request, CancellationToken ct = default)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateCreated = _dateTimeProvider.UtcNow,
            };
            var accounts = request.Accounts.Select(x => new Account
            {
                User = user,
                DateCreated = user.DateCreated,
            });
            user.Accounts = accounts;

            await _userService.AddAsync(user, ct);

            return Void.Value;
        }
    }

    public class CreateUser : Command
    {
        public string FirstName { get; }
        public string LastName { get; }
        public IEnumerable<Account> Accounts { get; } = new List<Account>();
    }
}
