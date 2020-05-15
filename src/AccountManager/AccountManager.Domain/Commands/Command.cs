using System;
using MediatR;

namespace AccountManager.Domain.Commands
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : Command
    {
    }

    public interface ICommandHandler<in TCommand, TResult>
        : IRequestHandler<TCommand, TResult> where TCommand : Command<TResult>
    {
    }

    public abstract class Command : BaseCommand, IRequest
    {
        protected Command() { }
        protected Command(Guid correlationId) : base(correlationId) { }
    }

    public abstract class Command<TResponse> : BaseCommand, IRequest<TResponse>
    {
        protected Command() { }
        protected Command(Guid correlationId) : base(correlationId) { }
    }

    public abstract class BaseCommand
    {
        protected BaseCommand()
        {
            CommandId = Guid.NewGuid();
            CorrelationId = CommandId;
        }

        protected BaseCommand(Guid correlationId)
        {
            CommandId = Guid.NewGuid();
            CorrelationId = correlationId;
        }

        public Guid CommandId { get; }
        public Guid CorrelationId { get; }
    }
}
