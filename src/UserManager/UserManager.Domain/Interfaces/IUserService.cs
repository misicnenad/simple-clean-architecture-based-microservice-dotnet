using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Domain.Models;

namespace UserManager.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> AddAsync(User user, CancellationToken ct = default);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default);
    }
}
