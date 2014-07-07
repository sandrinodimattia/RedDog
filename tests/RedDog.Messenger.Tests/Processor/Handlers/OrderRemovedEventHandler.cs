using System.Threading.Tasks;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Events;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class OrderRemovedEventHandler : IEventHandler<OrderCancelledEvent>, IEventHandler<OrderDeletedEvent>
    {
        public Task Handle(OrderCancelledEvent evt)
        {
            return Task.FromResult(false);
        }

        public Task Handle(OrderDeletedEvent evt)
        {
            return Task.FromResult(false);
        }
    }
}
