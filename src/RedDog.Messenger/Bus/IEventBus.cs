using System.Collections.Generic;
using System.Threading.Tasks;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        Task PublishAsync<TEvent>(IEnumerable<TEvent> events)
            where TEvent : class, IEvent;

        Task PublishAsync<TEvent>(Envelope<TEvent> envelope)
            where TEvent : class, IEvent;

        Task PublishAsync<TEvent>(Envelope<TEvent>[] envelopes)
            where TEvent : class, IEvent;
    }
}