using System.Threading.Tasks;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Events;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class OrderConfirmedEventHandler : IEventHandler<OrderConfirmedEvent>
    {
        public Task Handle(OrderConfirmedEvent evt)
        {
            return Task.FromResult(false);
        }
    }
}