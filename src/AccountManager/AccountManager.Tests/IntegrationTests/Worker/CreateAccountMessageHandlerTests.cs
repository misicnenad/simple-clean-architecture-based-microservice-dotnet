using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AccountManager.Infrastructure.Models;
using AccountManager.Tests.IntegrationTests.API;
using AccountManager.Worker.Handlers;

using Autofac;

using Microsoft.Extensions.Hosting;

using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Transport.InMem;

using Shared.Messages;

using Xunit;

namespace AccountManager.Tests.IntegrationTests.Worker
{
    public class CreateAccountMessageHandlerTests : TestFixture
    {
        private const string _queueName = "test";
        private const int _waitTimeInMiliseconds = 10000;

        private readonly EventWaitHandle _messageProccessedEvent = new ManualResetEvent(initialState: false);

        private readonly IBus _publisher;
        private readonly IHost _host;

        public CreateAccountMessageHandlerTests()
        {
            var subscriberStore = new InMemorySubscriberStore();
            var network = new InMemNetwork();

            hostBuilder.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterRebus((configurer, _) =>
                {
                    return configurer
                            .Transport(t => t.UseInMemoryTransport(network, _queueName))
                            .Subscriptions(s => s.StoreInMemory(subscriberStore))
                            .Options(b => b.SimpleRetryStrategy(maxDeliveryAttempts: 1))
                            .Events(e =>
                            {
                                e.AfterMessageHandled += (bus, hdrs, msg, ctx, args) => _messageProccessedEvent.Set();
                            });
                });

                builder.RegisterHandler<CreateAccountMessageHandler>();
            });

            var publisherActivator = new BuiltinHandlerActivator();
            _publisher = Configure.With(publisherActivator)
                .Transport(t => t.UseInMemoryTransportAsOneWayClient(network))
                .Subscriptions(s => s.StoreInMemory(subscriberStore))
                .Start();

            _host = hostBuilder.Start();
        }

        [Fact]
        public async Task Creates_Account()
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var userId = 1;
            var accountType = AccountType.Credit;
            var message = new CreateAccountMessage(correlationId, userId, accountType);

            // Act
            await _publisher.Publish(message);

            _messageProccessedEvent.WaitOne(_waitTimeInMiliseconds);

            // Assert
            using var context = _host.Resolve<AccountManagerDbContext>();
            const int minimumValidAccountId = 1;

            var account = context.Accounts.SingleOrDefault();
            Assert.NotNull(account);
            Assert.True(account.AccountId >= minimumValidAccountId);
            Assert.Equal(userId, account.UserId);
            Assert.Equal((int)accountType, (int)account.AccountType);
        }

        [Theory]
        [MemberData(nameof(InvalidMessages))]
        public async Task Invalid_Data_Throws(CreateAccountMessage invalidMessage)
        {
            // Act
            await _publisher.Publish(invalidMessage);

            _messageProccessedEvent.WaitOne(_waitTimeInMiliseconds);

            // Assert
            using var context = _host.Resolve<AccountManagerDbContext>();
            Assert.Empty(context.Accounts);
        }

        public static IEnumerable<object[]> InvalidMessages
        {
            get
            {
                yield return new CreateAccountMessage[]
                {
                    new CreateAccountMessage(Guid.Empty, -1, AccountType.None)
                };
                yield return new CreateAccountMessage[]
                {
                    new CreateAccountMessage(Guid.Empty, 1, AccountType.Credit)
                };
                yield return new CreateAccountMessage[]
                {
                    new CreateAccountMessage(Guid.NewGuid(), 0, AccountType.Credit)
                };
                yield return new CreateAccountMessage[]
                {
                    new CreateAccountMessage(Guid.NewGuid(), 1, AccountType.None)
                };
            }
        }
    }
}
