using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccountManager.Domain.Models;
using AccountManager.Infrastructure.Models;
using AccountManager.Infrastructure.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AccountManager.Tests.UnitTests.Infrastructure
{
    public class AccountServiceTests : IAsyncDisposable
    {
        private readonly ILoggerService<AccountService> _mockLoggerObject;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AccountManagerDbContext _dbContext;

        public AccountServiceTests()
        {
            _mockLoggerObject = new Mock<ILoggerService<AccountService>>().Object;
            _mockMapper = new Mock<IMapper>();

            var options = new DbContextOptions<AccountManagerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AccountManagerDbContext(options);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        [Fact]
        public async Task AddAsync_Successful()
        {
            // Arrange
            var accountToAdd = new Account
            {
                AccountType = AccountType.Credit,
                DateCreated = DateTime.UtcNow,
                UserId = 1,
            };
            var expectedAccountDbo = new AccountDbo
            {
                AccountType = accountToAdd.AccountType,
                DateCreated = accountToAdd.DateCreated,
                UserId = accountToAdd.UserId,
            };
            _mockMapper.Setup(m => m.Map<AccountDbo>(It.IsAny<Account>()))
                .Returns(expectedAccountDbo);

            var expectedAccount = new Account
            {
                AccountId = 1,
                AccountType = expectedAccountDbo.AccountType,
                DateCreated = expectedAccountDbo.DateCreated,
                UserId = expectedAccountDbo.UserId,
            };
            _mockMapper.Setup(m => m.Map<Account>(It.IsAny<AccountDbo>()))
                .Returns(expectedAccount);

            var service = new AccountService(_mockLoggerObject, _mockMapper.Object, _dbContext);

            // Act
            var correlationId = Guid.NewGuid();
            var newAccount = await service.AddAsync(correlationId, accountToAdd);

            // Assert
            Assert.NotNull(newAccount);
            Assert.Equal(expectedAccount.AccountId, newAccount.AccountId);
            Assert.Equal(expectedAccount.AccountType, newAccount.AccountType);
            Assert.Equal(expectedAccount.DateCreated, newAccount.DateCreated);
            Assert.Equal(expectedAccount.UserId, newAccount.UserId);
        }
    }
}
