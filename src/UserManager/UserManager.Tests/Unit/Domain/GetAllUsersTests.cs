using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Models;
using UserManager.Domain.Queries;
using Xunit;

namespace UserManager.Tests.Unit.Domain
{
    public class GetAllUsersTests
    {
        [Fact]
        public async Task Returns_All_Users()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User
                {
                    UserId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    DateCreated = DateTime.UtcNow,
                    Accounts = new List<Account>
                    {
                        new Account
                        {
                            AccountId = 1,
                            UserId = 1,
                            DateCreated = DateTime.UtcNow
                        }
                    }
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Papa",
                    LastName = "John",
                    DateCreated = DateTime.UtcNow,
                    Accounts = new List<Account>
                    {
                        new Account
                        {
                            AccountId = 2,
                            UserId = 2,
                            DateCreated = DateTime.UtcNow
                        }
                    }
                }
            };
            var mockService = new Mock<IUserService>();
            mockService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUsers);

            var handler = new GetAllUsersHandler(mockService.Object);

            //Act
            var request = new GetAllUsers();
            var result = await handler.HandleAsync(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsers, result);
        }
    }
}
