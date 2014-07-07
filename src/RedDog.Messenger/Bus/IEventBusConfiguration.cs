using System;

using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public interface IEventBusConfiguration : IBusConfiguration<IEventBusConfiguration>
    {
        /// <summary>
        /// Create the bus.
        /// </summary>
        /// <returns></returns>
        IEventBus Build();

        /// <summary>
        /// Register a single event with a sender.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        IEventBusConfiguration RegisterEvent<TEvent>(IMessageSender sender)
            where TEvent : IEvent;

        /// <summary>
        /// Register one or more events with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        IEventBusConfiguration RegisterEvents(IMessageSender sender, Action<IFluentMessageRegistration<IEvent>> config);

        /// <summary>
        /// Register an array of events with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEventBusConfiguration RegisterEvents(IMessageSender sender, params Type[] types);

        /// <summary>
        /// Register one or more events with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        IEventBusConfiguration RegisterEvents(IMessageSender sender, IMessageRegistration<IEvent> registrationSource);
    }
}