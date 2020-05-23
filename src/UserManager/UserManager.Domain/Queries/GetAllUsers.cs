using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Models;

namespace UserManager.Domain.Queries
{
    public class GetAllUsersHandler : QueryHandler<GetAllUsers, IEnumerable<User>>
    {
        private readonly IUserService _userService;

        public GetAllUsersHandler(
            IUserService userService, 
            IEnumerable<IPreProcessHandler<GetAllUsers, IEnumerable<User>>> preProcessHandlers)
            : base(preProcessHandlers)
        {
            _userService = userService;
        }

        protected override async Task<IEnumerable<User>> HandlerInternalAsync(GetAllUsers request, CancellationToken ct = default)
        {
            return await _userService.GetAllAsync(ct);
        }
    }

    public class GetAllUsers : Query<IEnumerable<User>>
    {
    }
}
