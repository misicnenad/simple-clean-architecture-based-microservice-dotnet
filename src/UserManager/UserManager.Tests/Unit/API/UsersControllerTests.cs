using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserManager.API.Controllers;
using UserManager.API.Models;
using UserManager.Domain.Models;
using UserManager.Domain.Queries;
using Xunit;

namespace UserManager.Tests.Unit.API
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetAllUsersAsync_Returns_Users()
        {
            //Arrange
            var expectedUsers = new List<User> { new User(), new User(), };
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllUsers>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUsers);

            var expectedUserDtos = expectedUsers.Select(_ => new UserDto());
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                .Returns(expectedUserDtos);

            var controller = new UsersController(mockMediator.Object, mockMapper.Object);

            //Act
            var result = await controller.GetAllUsersAsync();

            //Assert
            Assert.NotNull(result);

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var userDtos = okResult.Value as IEnumerable<UserDto>;
            Assert.NotNull(userDtos);
            Assert.Equal(expectedUserDtos, userDtos);
        }

        [Fact]
        public async Task GetAllUsersAsync_Returns_Empty_Collection()
        {
            //Arrange
            var expectedUsers = new List<User>();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllUsers>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUsers);

            var expectedUserDtos = expectedUsers.Select(_ => new UserDto());
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                .Returns(expectedUserDtos);

            var controller = new UsersController(mockMediator.Object, mockMapper.Object);

            //Act
            var result = await controller.GetAllUsersAsync();

            //Assert
            Assert.NotNull(result);

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var userDtos = okResult.Value as IEnumerable<UserDto>;
            Assert.NotNull(userDtos);
            Assert.Empty(userDtos);
        }
    }
}
