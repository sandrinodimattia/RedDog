using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RedDog.Messenger.Contracts;
using RedDog.Messenger.Diagnostics;

namespace RedDog.Messenger.Bus
{
    public class EventBus : MessageBus, IEventBus
    {
        public EventBus(IBusConfiguration<IEventBusConfiguration> configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Send a event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            return PublishAsync(new Envelope<TEvent>(@event));
        }

        /// <summary>
        /// Send multiple events.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(IEnumerable<TEvent> events)
            where TEvent : class, IEvent
        {
            return PublishAsync(events.Select(@event => new Envelope<TEvent>(@event)).ToArray());
        }

        /// <summary>
        /// Send a event wrapped in an envelope.
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        public async Task PublishAsync<TEvent>(Envelope<TEvent> envelope)
            where TEvent : class, IEvent
        {
            try
            {
                MessengerEventSource.Log.Sending(envelope.Body.GetType(), envelope);

                // Send.
                var sender = Configuration
                    .GetSender(typeof(TEvent));
                await sender
                    .SendAsync(await BuildBrokeredMessage(envelope))
                    .ConfigureAwait(false);

                // Complete.
                MessengerEventSource.Log.Sent(typeof(TEvent), sender, envelope);

            }
            catch (Exception ex)
            {
                MessengerEventSource.Log.SendFailed(envelope.Body.GetType(), ex);

                // Rethrow.
                throw;
            }
        }

        /// <summary>
        /// Send multiple events each wrapped in an envelope.
        /// </summary>
        /// <param name="envelopes"></param>
        /// <returns></returns>
        public async Task PublishAsync<TEvent>(Envelope<TEvent>[] envelopes)
            where TEvent : class, IEvent
        {
            try
            {
                foreach (var envelope in envelopes)
                {
                    MessengerEventSource.Log.Sending(typeof(TEvent), envelope);
                }

                // Send batch.
                var sender = Configuration
                    .GetSender(typeof(TEvent));
                await sender
                    .SendBatchAsync(await Task.WhenAll(envelopes.Select(async @event => await BuildBrokeredMessage(@event))))
                    .ConfigureAwait(false);

                // Complete.
                MessengerEventSource.Log.BatchSent(typeof(TEvent), envelopes.Length, sender);
            }
            catch (Exception ex)
            {
                MessengerEventSource.Log.SendBatchFailed(typeof(TEvent), envelopes.Length, ex);

                // Rethrow.
                throw;
            }
        }
    }
}