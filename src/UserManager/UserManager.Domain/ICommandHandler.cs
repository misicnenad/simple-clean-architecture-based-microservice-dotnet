namespace UserManager.Domain
{
    public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Void> 
        where TCommand : Command
    {
    }

    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult> 
        where TCommand : Command<TResult>
    {
    }
}
