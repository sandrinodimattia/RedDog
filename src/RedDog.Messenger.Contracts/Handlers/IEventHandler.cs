using System.Threading.Tasks;

namespace RedDog.Messenger.Contracts.Handlers
{
    public interface IEventHandler : IMessageHandler
    {

    }

    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : class, IEvent
    {
        Task Handle(TEvent @event);
    }
}
