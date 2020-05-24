using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Models;

namespace UserManager.Domain.Queries
{
    public class GetAllUsersHandler : IQueryHandler<GetAllUsers, IEnumerable<User>>
    {
        private readonly IUserService _userService;

        public GetAllUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<User>> HandleAsync(GetAllUsers request, CancellationToken ct = default)
        {
            return await _userService.GetAllAsync(ct);
        }
    }

    public class GetAllUsers : Query<IEnumerable<User>>
    {
    }
}
