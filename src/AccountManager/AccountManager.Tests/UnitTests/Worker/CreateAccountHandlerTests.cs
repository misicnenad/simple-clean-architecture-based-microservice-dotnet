using System;
using System.Threading;
using System.Threading.Tasks;

using AccountManager.Domain.Commands;
using AccountManager.Worker.Handlers;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shared.Messages;

using Xunit;

namespace AccountManager.Tests.UnitTests.Worker
{
    public class CreateAccountHandlerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ILogger<CreateAccountMessageHandler> _mockLoggerObject;

        public CreateAccountHandlerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMapper = new Mock<IMapper>();
            _mockLoggerObject = new Mock<ILogger<CreateAccountMessageHandler>>().Object;
        }

        [Fact]
        public async Task CreatesAccount()
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var userId = 1;
            var accountType = AccountType.Credit;

            var expectedCommand = new CreateAccount(correlationId, userId, AccountManager.Domain.Models.AccountType.Credit);
            _mockMapper.Setup(x => x.Map<CreateAccount>(It.IsAny<CreateAccountMessage>()))
                .Returns(expectedCommand);

            _mockMediator.Setup(x => x.Send(It.Is<CreateAccount>(r => r == expectedCommand), It.IsAny<CancellationToken>()))
                .Verifiable();

            var handler = new CreateAccountMessageHandler(_mockMediator.Object, _mockMapper.Object, _mockLoggerObject);

            // Act
            var message = new CreateAccountMessage(correlationId, userId, accountType);
            await handler.Handle(message);

            // Assert
            _mockMediator.Verify();
        }
    }
}
