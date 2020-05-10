using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Commands;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Domain.Providers;
using Moq;
using Xunit;

namespace AccountManager.Tests.UnitTests.Domain
{
    public class CreateAccountTests
    {
        private static readonly DateTime _expectedDateTime = DateTime.UtcNow;

        private readonly Mock<IAccountService> _mockAccountService;
        private readonly DateTimeProvider _mockDateTimeProviderObject;

        public CreateAccountTests()
        {
            _mockAccountService = new Mock<IAccountService>();

            var mockDateTimeProvider = new Mock<DateTimeProvider>();
            mockDateTimeProvider.Setup(p => p.UtcNow)
                .Returns(_expectedDateTime);
            _mockDateTimeProviderObject = mockDateTimeProvider.Object;
        }

        [Fact]
        public async Task Creates_Account()
        {
            // Arrange
            var expectedAccount = new Account
            {
                AccountId = 1,
                AccountType = AccountType.Credit,
                DateCreated = _expectedDateTime,
                UserId = 1,
            };
            _mockAccountService.Setup(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAccount);

            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockDateTimeProviderObject);

            // Act
            var request = new CreateAccount(userId: expectedAccount.UserId, expectedAccount.AccountType);
            var newAccount = await handler.Handle(request);

            // Assert
            Assert.NotNull(newAccount);
            Assert.Equal(expectedAccount.AccountId, newAccount.AccountId);
            Assert.Equal(expectedAccount.AccountType, newAccount.AccountType);
            Assert.Equal(expectedAccount.DateCreated, newAccount.DateCreated);
            Assert.Equal(expectedAccount.UserId, newAccount.UserId);
        }

        [Fact]
        public async Task Null_Request_Throws()
        {
            // Arrange
            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockDateTimeProviderObject);

            // Act
            CreateAccount invalidRequest = null;
            var exception = await Record.ExceptionAsync(() => handler.Handle(invalidRequest));

            // Assert
            _mockAccountService.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.NotNull(exception);
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task IAccountService_Throws()
        {
            // Arrange
            _mockAccountService.Setup(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockDateTimeProviderObject);

            // Act
            var request = new CreateAccount(userId: 1, AccountType.Credit);
            var exception = await Record.ExceptionAsync(() => handler.Handle(request));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
        }
    }
}
