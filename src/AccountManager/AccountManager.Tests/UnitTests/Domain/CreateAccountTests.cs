using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Commands;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Domain.Providers;
using AccountManager.Domain.Validators;
using FluentValidation;
using Moq;
using Xunit;

namespace AccountManager.Tests.UnitTests.Domain
{
    public class CreateAccountTests
    {
        private static readonly DateTime _expectedDateTime = DateTime.UtcNow;

        private readonly Mock<IAccountService> _mockAccountService;
        private readonly DateTimeProvider _mockDateTimeProviderObject;
        private readonly Mock<Validator<CreateAccount>> _mockValidationRules;

        public CreateAccountTests()
        {
            _mockAccountService = new Mock<IAccountService>();

            _mockValidationRules = new Mock<Validator<CreateAccount>>();
            _mockValidationRules.Setup(vr => vr.ValidateAndThrow(It.IsAny<CreateAccount>()))
                .Verifiable();

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
            _mockAccountService.Setup(s => s.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAccount);

            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockValidationRules.Object, _mockDateTimeProviderObject);

            // Act
            var request = new CreateAccount(userId: expectedAccount.UserId, expectedAccount.AccountType);
            var newAccount = await handler.Handle(request);

            // Assert
            _mockValidationRules.Verify();

            Assert.NotNull(newAccount);
            Assert.Equal(expectedAccount.AccountId, newAccount.AccountId);
            Assert.Equal(expectedAccount.AccountType, newAccount.AccountType);
            Assert.Equal(expectedAccount.DateCreated, newAccount.DateCreated);
            Assert.Equal(expectedAccount.UserId, newAccount.UserId);
        }

        [Fact]
        public async Task Invalid_Request_Throws()
        {
            // Arrange
            _mockValidationRules.Setup(vr => vr.ValidateAndThrow(It.IsAny<CreateAccount>()))
                .Throws(new ValidationException(string.Empty));

            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockValidationRules.Object, _mockDateTimeProviderObject);

            // Act
            var invalidRequest = new CreateAccount(userId: default, accountType: default);
            var exception = await Record.ExceptionAsync(() => handler.Handle(invalidRequest));

            // Assert
            _mockAccountService.Verify(s => s.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public async Task Null_Request_Throws()
        {
            // Arrange
            _mockValidationRules.Setup(vr => vr.ValidateAndThrow(It.IsAny<CreateAccount>()))
                .Throws(new ArgumentNullException());

            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockValidationRules.Object, _mockDateTimeProviderObject);

            // Act
            CreateAccount invalidRequest = null;
            var exception = await Record.ExceptionAsync(() => handler.Handle(invalidRequest));

            // Assert
            _mockAccountService.Verify(s => s.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task IAccountService_Throws()
        {
            // Arrange
            _mockAccountService.Setup(s => s.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = new CreateAccountHandler(_mockAccountService.Object, _mockValidationRules.Object, _mockDateTimeProviderObject);

            // Act
            var request = new CreateAccount(userId: 1, AccountType.Credit);
            var exception = await Record.ExceptionAsync(() => handler.Handle(request));

            // Assert
            _mockValidationRules.Verify();

            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
        }
    }
}
