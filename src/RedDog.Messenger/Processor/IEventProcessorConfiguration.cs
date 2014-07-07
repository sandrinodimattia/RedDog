using System;

using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;

namespace RedDog.Messenger.Processor
{
    public interface IEventProcessorConfiguration : IProcessorConfiguration<IEventProcessorConfiguration>
    {
        /// <summary>
        /// Create the processor.
        /// </summary>
        /// <returns></returns>
        IEventProcessor Build();

        /// <summary>
        /// Register a single handler with a receiver.
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="receiver"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandler<TEventHandler>(IMessagePump receiver)
            where TEventHandler : IEventHandler;

        /// <summary>
        /// Register one or more handler with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandlers(IMessagePump receiver, Action<IFluentMessageHandlerRegistration<IEventHandler>> config);

        /// <summary>
        /// Register an array of handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandlers(IMessagePump receiver, params Type[] types);

        /// <summary>
        /// Register one or more handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandlers(IMessagePump receiver, IMessageHandlerRegistration<IEventHandler> registrationSource);

        /// <summary>
        /// Register a single handler with a receiver.
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="receiver"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandler<TEventHandler>(ISessionMessagePump receiver)
            where TEventHandler : IEventHandler;

        /// <summary>
        /// Register one or more handler with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandlers(ISessionMessagePump receiver, Action<IFluentMessageHandlerRegistration<IEventHandler>> config);

        /// <summary>
        /// Register an array of handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandlers(ISessionMessagePump receiver, params Type[] types);

        /// <summary>
        /// Register one or more handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        IEventProcessorConfiguration RegisterEventHandlers(ISessionMessagePump receiver, IMessageHandlerRegistration<IEventHandler> registrationSource);
    }
}