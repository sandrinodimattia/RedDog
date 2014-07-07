using System;

using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;

namespace RedDog.Messenger.Processor
{
    public interface ICommandProcessorConfiguration : IProcessorConfiguration<ICommandProcessorConfiguration>
    {
        /// <summary>
        /// Create the processor.
        /// </summary>
        /// <returns></returns>
        ICommandProcessor Build();

        /// <summary>
        /// Register a single handler with a receiver.
        /// </summary>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <param name="receiver"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandler<TCommandHandler>(IMessagePump receiver)
            where TCommandHandler : ICommandHandler;

        /// <summary>
        /// Register one or more handler with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, Action<IFluentMessageHandlerRegistration<ICommandHandler>> config);

        /// <summary>
        /// Register an array of handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, params Type[] types);

        /// <summary>
        /// Register one or more handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, IMessageHandlerRegistration<ICommandHandler> registrationSource);

        /// <summary>
        /// Register a single handler with a receiver.
        /// </summary>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <param name="receiver"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandler<TCommandHandler>(ISessionMessagePump receiver)
            where TCommandHandler : ICommandHandler;

        /// <summary>
        /// Register one or more handler with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, Action<IFluentMessageHandlerRegistration<ICommandHandler>> config);

        /// <summary>
        /// Register an array of handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, params Type[] types);

        /// <summary>
        /// Register one or more handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, IMessageHandlerRegistration<ICommandHandler> registrationSource);
    }
}