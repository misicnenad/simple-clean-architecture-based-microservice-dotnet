using System;
using MediatR;

namespace AccountManager.Domain.Commands
{
    public interface ICommand : IRequest
    {
        public Guid CommandId { get; }
        public Guid CorrelationId { get; }
    }

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : class, ICommand
    {
    }

    public interface ICommand<TResponse> : IRequest<TResponse>
    {
        public Guid CommandId { get; }
        public Guid CorrelationId { get; }
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
    }
}
