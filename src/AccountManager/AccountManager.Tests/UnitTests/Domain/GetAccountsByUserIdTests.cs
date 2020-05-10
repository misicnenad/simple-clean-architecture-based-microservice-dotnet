using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Domain.Queries;
using Moq;
using Xunit;

namespace AccountManager.Tests.UnitTests.Domain
{
    public class GetAccountsByUserIdTests
    {
        private readonly Mock<IAccountService> _mockAccountService;

        public GetAccountsByUserIdTests()
        {
            _mockAccountService = new Mock<IAccountService>();
        }

        [Fact]
        public async Task Returns_Accounts()
        {
            // Arrange
            var userId = 1;
            var expectedAccounts = new List<Account>
            {
                new Account
                {
                    AccountId = 1,
                    UserId = userId,
                    AccountType = AccountType.Credit,
                    DateCreated = DateTime.UtcNow
                },
                new Account
                {
                    AccountId = 2,
                    UserId = userId,
                    AccountType = AccountType.Debit,
                    DateCreated = DateTime.UtcNow.AddDays(-1)
                },
                new Account
                {
                    AccountId = 3,
                    UserId = userId,
                    AccountType = AccountType.Credit,
                    DateCreated = DateTime.UtcNow.AddDays(-3)
                }
            };
            _mockAccountService.Setup(
                    s => s.GetAllByUserIdAsync(It.IsAny<Guid>(), It.Is<int>(id => id == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAccounts);

            var handler = new GetAccountsByUserIdHandler(_mockAccountService.Object);

            // Act
            var request = new GetAccountsByUserId(userId);
            var accounts = await handler.Handle(request);

            // Assert
            Assert.NotNull(accounts);
            Assert.NotEmpty(accounts);
            Assert.Equal(expectedAccounts.Intersect(accounts).Count(), accounts.Count());
        }

        [Fact]
        public async Task Null_Request_Throws()
        {
            // Arrange
            var handler = new GetAccountsByUserIdHandler(_mockAccountService.Object);

            // Act
            GetAccountsByUserId invalidRequest = null;
            var exception = await Record.ExceptionAsync(() => handler.Handle(invalidRequest));

            // Assert
            _mockAccountService.Verify(s => s.GetAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.NotNull(exception);
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task IAccountService_Throws()
        {
            // Arrange
            _mockAccountService.Setup(s => s.GetAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = new GetAccountsByUserIdHandler(_mockAccountService.Object);

            // Act
            var request = new GetAccountsByUserId(userId: 1);
            var exception = await Record.ExceptionAsync(() => handler.Handle(request));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
        }
    }
}
