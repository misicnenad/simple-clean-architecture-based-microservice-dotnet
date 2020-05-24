using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserManager.Domain.Models;
using UserManager.Infrastructure.Models;
using UserManager.Infrastructure.Services;
using Xunit;

namespace UserManager.Tests.Unit.Infrastructure
{
    public class UserServiceTests
    {
        private readonly DbContextOptions<UserManagerDbContext> _options =
            new DbContextOptionsBuilder<UserManagerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        [Fact]
        public async Task GetAllUsersAsync_Returns_Users()
        {
            // Arrange
            var expectedUserDbos = new List<UserDbo>
            {
                new UserDbo
                {
                    UserId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Accounts = new List<AccountDbo>
                    {
                        new AccountDbo
                        {
                            AccountId = 1,
                        }
                    }
                },
                new UserDbo
                {
                    UserId = 2,
                    FirstName = "Papa",
                    LastName = "John",
                    Accounts = new List<AccountDbo>
                    {
                        new AccountDbo
                        {
                            AccountId = 2,
                        }
                    }
                }
            };
            using var dbContext = new UserManagerDbContext(_options);
            dbContext.Users.AddRange(expectedUserDbos);
            dbContext.SaveChanges();

            var expectedUsers = expectedUserDbos.Select(x => new User
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateCreated = x.DateCreated,
            });
            _mockMapper.Setup(m => m.Map<IEnumerable<User>>(It.IsAny<IEnumerable<UserDbo>>()))
                .Returns(expectedUsers);

            var service = new UserService(_mockMapper.Object, dbContext);

            // Act
            var actualUsers = await service.GetAllAsync();

            // Assert
            Assert.NotNull(actualUsers);
            Assert.NotEmpty(actualUsers);
            Assert.Equal(expectedUsers, actualUsers);
        }

        [Fact]
        public async Task GetAllByUserIdAsync_Returns_Empty_Collection()
        {
            // Arrange
            using var dbContext = new UserManagerDbContext(_options);

            var expectedUsers = new List<User>();
            _mockMapper.Setup(m => m.Map<IEnumerable<User>>(It.IsAny<IEnumerable<UserDbo>>()))
                .Returns(expectedUsers);

            var service = new UserService(_mockMapper.Object, dbContext);

            // Act
            var actualUsers = await service.GetAllAsync();

            // Assert
            Assert.NotNull(actualUsers);
            Assert.Empty(actualUsers);
        }
    }
}
