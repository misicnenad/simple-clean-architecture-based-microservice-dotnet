using System.Collections.Generic;
using UserManager.Domain.Commands;

namespace UserManager.Domain
{
    public abstract class CommandHandler<TCommand>
        : BaseRequestHandler<TCommand> where TCommand : Command
    {
        protected CommandHandler(IEnumerable<IPreProcessHandler<TCommand>> preProcessHandlers) : 
            base(preProcessHandlers)
        {
        }
    }

    public abstract class CommandHandler<TCommand, TResult>
        : BaseRequestHandler<TCommand, TResult> where TCommand : Command<TResult>
    {
        protected CommandHandler(IEnumerable<IPreProcessHandler<TCommand, TResult>> preProcessHandlers)
            : base(preProcessHandlers)
        {
        }
    }
}
