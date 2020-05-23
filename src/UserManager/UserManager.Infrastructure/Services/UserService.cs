using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Models;
using UserManager.Infrastructure.Models;

namespace UserManager.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManagerDbContext _dbContext;

        public UserService(IMapper mapper, UserManagerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
        {
            var userDbos = await _dbContext.Users.ToListAsync(ct);

            var users = _mapper.Map<IEnumerable<User>>(userDbos);

            return users;
        }
    }
}
