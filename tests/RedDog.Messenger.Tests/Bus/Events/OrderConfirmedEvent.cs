using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Events
{
    public class OrderConfirmedEvent : IEvent
    {
        public string Id
        {
            get;
            set;
        }
    }
}