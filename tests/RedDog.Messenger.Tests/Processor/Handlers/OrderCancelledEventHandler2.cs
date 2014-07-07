using System;
using System.Threading.Tasks;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Events;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class OrderCancelledEventHandler2 : IEventHandler<OrderCancelledEvent>
    {
        private readonly Action _action;

        public OrderCancelledEventHandler2(Action action)
        {
            _action = action;
        }

        public Task Handle(OrderCancelledEvent @event)
        {
            _action();
            return Task.FromResult(0);
        }
    }
}