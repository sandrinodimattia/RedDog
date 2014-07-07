using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Events
{
    public class OrderCancelledEvent : IEvent
    {
        public string Id
        {
            get;
            set;
        }
    }
}