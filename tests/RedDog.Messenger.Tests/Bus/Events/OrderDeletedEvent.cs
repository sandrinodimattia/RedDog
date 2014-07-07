using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Events
{
    public class OrderDeletedEvent : IEvent
    {
        public string Id
        {
            get;
            set;
        }
    }
}
