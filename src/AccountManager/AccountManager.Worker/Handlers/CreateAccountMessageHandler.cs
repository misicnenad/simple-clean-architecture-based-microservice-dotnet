using System.Threading.Tasks;
using Rebus.Handlers;
using MediatR;
using AccountManager.Domain.Commands;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Rebus.Retry.Simple;
using Shared.Messages;

namespace AccountManager.Worker.Handlers
{
    public class CreateAccountMessageHandler : IHandleMessages<CreateAccountMessage>, IHandleMessages<IFailed<CreateAccountMessage>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAccountMessageHandler> _logger;

        public CreateAccountMessageHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<CreateAccountMessageHandler> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(CreateAccountMessage message)
        {
            _logger.LogInformation($"Received {nameof(CreateAccountMessage)} with ID: {message.CorrelationId}.");

            var command = _mapper.Map<CreateAccount>(message);

            await _mediator.Send(command);
        }

        public async Task Handle(IFailed<CreateAccountMessage> message)
        {
            _logger.LogError($"{nameof(CreateAccountMessage)} failed with {nameof(message.Message.CorrelationId)}: " +
                $"{message.Message.CorrelationId} and error description {message.ErrorDescription}");

            await Task.CompletedTask;
        }
    }
}
