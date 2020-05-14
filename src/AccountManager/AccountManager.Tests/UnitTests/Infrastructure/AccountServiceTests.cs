using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
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
        private readonly Mock<IMapper> _mockMapper;
        private readonly AccountManagerDbContext _dbContext;

        public AccountServiceTests()
        {
            _mockMapper = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<AccountManagerDbContext>()
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

            var service = new AccountService(_mockMapper.Object, _dbContext);

            // Act
            var correlationId = Guid.NewGuid();
            var actualAccount = await service.AddAsync(correlationId, accountToAdd);

            // Assert
            Assert.NotNull(actualAccount);
            Assert.Equal(expectedAccount.AccountId, actualAccount.AccountId);
            Assert.Equal(expectedAccount.AccountType, actualAccount.AccountType);
            Assert.Equal(expectedAccount.DateCreated, actualAccount.DateCreated);
            Assert.Equal(expectedAccount.UserId, actualAccount.UserId);
        }

        [Fact]
        public async Task GetAllByUserIdAsync_Returns_Accounts()
        {
            // Arrange
            var userId = 1;
            var account1 = new AccountDbo
            {
                AccountId = 1,
                UserId = userId,
                AccountType = AccountType.Credit,
                DateCreated = DateTime.UtcNow
            };
            var account2 = new AccountDbo
            {
                AccountId = 2,
                UserId = userId,
                AccountType = AccountType.Debit,
                DateCreated = DateTime.UtcNow.AddDays(-1)
            };
            var accountDbos = new List<AccountDbo>
            {
                account1,
                account2
            };
            _dbContext.Accounts.AddRange(accountDbos);

            var expectedAccounts = new List<Account>
            {
                new Account
                {
                    AccountId = account1.AccountId,
                    UserId = userId,
                    AccountType = account1.AccountType,
                    DateCreated = account1.DateCreated
                },
                new Account
                {
                    AccountId = account2.AccountId,
                    UserId = userId,
                    AccountType = account2.AccountType,
                    DateCreated = account2.DateCreated
                },
            };
            _mockMapper.Setup(
                    m => m.Map<IEnumerable<Account>>(
                        It.Is<List<AccountDbo>>(accounts => accounts.All(a => accountDbos.Contains(a)))))
                .Returns(expectedAccounts);

            var service = new AccountService(_mockMapper.Object, _dbContext);

            // Act
            var actualAccounts = await service.GetAllByUserIdAsync(correlationId: Guid.NewGuid(), userId);

            // Assert
            Assert.NotNull(actualAccounts);
            Assert.NotEmpty(actualAccounts);
            Assert.True(actualAccounts.All(a => expectedAccounts.Contains(a)));
        }

        [Fact]
        public async Task GetAllByUserIdAsync_Returns_Empty_Collection()
        {
            // Arrange
            var accountDbos = new List<AccountDbo>
            {
                new AccountDbo
                {
                    AccountId = 1,
                    UserId = 1,
                    AccountType = AccountType.Credit,
                    DateCreated = DateTime.UtcNow
                },
                new AccountDbo
                {
                    AccountId = 2,
                    UserId = 2,
                    AccountType = AccountType.Debit,
                    DateCreated = DateTime.UtcNow
                },
            };
            _dbContext.Accounts.AddRange(accountDbos);

            var expectedAccounts = new List<Account>();
            _mockMapper.Setup(
                    m => m.Map<IEnumerable<Account>>(
                        It.Is<List<AccountDbo>>(accountDbos => !accountDbos.Any())))
                .Returns(expectedAccounts);

            var service = new AccountService(_mockMapper.Object, _dbContext);

            // Act
            var nonExistentUserId = 3;
            var actualAccounts = await service.GetAllByUserIdAsync(correlationId: Guid.NewGuid(), nonExistentUserId);

            // Assert
            Assert.NotNull(actualAccounts);
            Assert.Empty(actualAccounts);
        }
    }
}
