using UserManager.Domain.Commands;

namespace UserManager.Domain
{
    public abstract class CommandHandler<TCommand>
        : BaseRequestHandler<TCommand> where TCommand : Command
    {
        protected CommandHandler(IPreProcessHandler<TCommand> preProcessHandler) 
            : base(preProcessHandler)
        {
        }
    }

    public abstract class CommandHandler<TCommand, TResult>
        : BaseRequestHandler<TCommand, TResult> where TCommand : Command<TResult>
    {
        protected CommandHandler(IPreProcessHandler<TCommand, TResult> preProcessHandler)
            : base(preProcessHandler)
        {
        }
    }
}
