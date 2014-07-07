using System.Threading.Tasks;

namespace RedDog.Messenger.Contracts.Handlers
{
    public interface ICommandHandler : IMessageHandler
    {

    }

    public interface ICommandHandler<in TCommand> : ICommandHandler
        where TCommand : class, ICommand
    {
        Task Handle(TCommand command);
    }
}
