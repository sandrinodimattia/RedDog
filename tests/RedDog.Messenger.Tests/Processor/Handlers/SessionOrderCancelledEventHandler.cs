using System.Threading.Tasks;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Events;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class SessionOrderCancelledEventHandler : IEventHandler<OrderCancelledEvent>, ISessionHandler
    {
        public Task Handle(OrderCancelledEvent @event)
        {
            return Task.FromResult(0);
        }

        public ISession Session
        {
            get;
            set;
        }
    }
}