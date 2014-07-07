using System;

using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public class EventBusBuilder : BusBuilder<IEventBusConfiguration>, IEventBusConfiguration
    {
        /// <summary>
        /// Build the bus.
        /// </summary>
        /// <returns></returns>
        public IEventBus Build()
        {
            return new EventBus(this);
        }

        /// <summary>
        /// Register a single event with a sender.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        public IEventBusConfiguration RegisterEvent<TEvent>(IMessageSender sender)
            where TEvent : IEvent
        {
            return RegisterEvents(sender, new TypeMessageRegistration<IEvent>(typeof(TEvent)));
        }

        /// <summary>
        /// Register one or more events with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public IEventBusConfiguration RegisterEvents(IMessageSender sender, Action<IFluentMessageRegistration<IEvent>> config)
        {
            // Start the fluent configuration.
            var registration = new FluentMessageRegistration<IEvent>();
            config(registration);

            // Register.
            return RegisterEvents(sender, registration);
        }

        /// <summary>
        /// Register an array of events with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public IEventBusConfiguration RegisterEvents(IMessageSender sender, params Type[] types)
        {
            return RegisterEvents(sender, new ArrayMessageRegistration<IEvent>(types));
        }

        /// <summary>
        /// Register one or more events with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        public IEventBusConfiguration RegisterEvents(IMessageSender sender, IMessageRegistration<IEvent> registrationSource)
        {
            foreach (var type in registrationSource.GetTypes())
            {
                AddMessageType(type, sender);
            }

            return this;
        }

        /// <summary>
        /// Create the EventBus builder.
        /// </summary>
        /// <returns></returns>
        public static IEventBusConfiguration Create()
        {
            return new EventBusBuilder();
        }
    }
}