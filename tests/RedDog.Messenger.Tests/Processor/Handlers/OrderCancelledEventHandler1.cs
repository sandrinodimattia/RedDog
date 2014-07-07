using System;
using System.Threading.Tasks;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Events;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class OrderCancelledEventHandler1 : IEventHandler<OrderCancelledEvent>
    {
        private readonly Action _action;

        public OrderCancelledEventHandler1(Action action)
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