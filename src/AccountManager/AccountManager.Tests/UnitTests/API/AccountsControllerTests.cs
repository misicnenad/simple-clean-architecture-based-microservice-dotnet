using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.API.Controllers;
using AccountManager.API.Models;
using AccountManager.Domain.Models;
using AccountManager.Domain.Queries;
using AccountManager.Infrastructure.Models;
using AutoMapper;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AccountManager.Tests.UnitTests.API
{
    public class AccountsControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IMapper> _mockMapper;

        public AccountsControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAccountsByUserIdAsync_Returns_Accounts()
        {
            // Arrange
            var userId = 1;
            var account1 = new Account
            {
                AccountId = 1,
                UserId = userId,
                AccountType = AccountType.Credit,
                DateCreated = DateTime.UtcNow,
            };
            var account2 = new Account
            {
                AccountId = 2,
                UserId = userId,
                AccountType = AccountType.Debit,
                DateCreated = DateTime.UtcNow,
            };
            var expectedAccounts = new List<Account>
            {
                account1,
                account2
            };
            _mockMediator.Setup(x => x.Send(It.IsAny<GetAccountsByUserId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAccounts);

            var expectedAccountDtos = new List<AccountDto>
            {
                new AccountDto
                {
                    AccountId = account1.AccountId,
                    UserId = account1.UserId,
                    AccountType = account1.AccountType,
                    DateCreated = account1.DateCreated,
                },
                new AccountDto
                {
                    AccountId = account2.AccountId,
                    UserId = account2.UserId,
                    AccountType = account2.AccountType,
                    DateCreated = account2.DateCreated,
                },
            };
            _mockMapper.Setup(
                x => x.Map<IEnumerable<AccountDto>>(
                    It.Is<IEnumerable<Account>>(accs => accs == expectedAccounts)))
                .Returns(expectedAccountDtos);

            var controller = new AccountsController(_mockMediator.Object, _mockMapper.Object);

            // Act
            var actionResult = await controller.GetAccountsByUserIdAsync(userId);

            // Assert
            var result = actionResult?.Result as OkObjectResult;
            Assert.NotNull(result);
            
            Assert.Equal(expectedAccountDtos, result.Value);
        }

        [Fact]
        public async Task GetAccountsByUserIdAsync_Returns_Empty_Collection()
        {
            // Arrange
            var userId = 1;
            var expectedAccounts = new List<Account>();
            _mockMediator.Setup(x => x.Send(It.IsAny<GetAccountsByUserId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAccounts);

            var expectedAccountDtos = new List<AccountDto>();
            _mockMapper.Setup(
                x => x.Map<IEnumerable<AccountDto>>(
                    It.Is<IEnumerable<Account>>(accs => accs == expectedAccounts)))
                .Returns(expectedAccountDtos);

            var controller = new AccountsController(_mockMediator.Object, _mockMapper.Object);

            // Act
            var actionResult = await controller.GetAccountsByUserIdAsync(userId);

            // Assert
            var result = actionResult?.Result as OkObjectResult;
            Assert.NotNull(result);

            var value = result.Value as IEnumerable<AccountDto>;
            Assert.NotNull(value);
            Assert.Empty(value);
        }
    }
}
