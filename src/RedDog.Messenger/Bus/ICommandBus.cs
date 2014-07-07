using System.Collections.Generic;
using System.Threading.Tasks;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus
{
    public interface ICommandBus
    {
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand;

        Task SendAsync<TCommand>(IEnumerable<TCommand> commands)
            where TCommand : class, ICommand;

        Task SendAsync<TCommand>(Envelope<TCommand> envelope)
            where TCommand : class, ICommand;

        Task SendAsync<TCommand>(Envelope<TCommand>[] envelopes)
            where TCommand : class, ICommand;
    }
}