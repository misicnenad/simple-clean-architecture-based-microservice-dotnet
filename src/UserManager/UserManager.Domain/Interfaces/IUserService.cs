using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Domain.Models;

namespace UserManager.Domain.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default);
    }
}
