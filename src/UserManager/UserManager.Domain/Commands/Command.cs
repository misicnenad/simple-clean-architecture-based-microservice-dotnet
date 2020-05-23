using System;

namespace UserManager.Domain.Commands
{
    public abstract class Command : Command<Void>
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
