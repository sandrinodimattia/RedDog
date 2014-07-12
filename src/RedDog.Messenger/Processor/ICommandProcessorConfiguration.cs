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
        /// Register one or more handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, IMessageHandlerRegistration<ICommandHandler> registrationSource);

        /// <summary>
        /// Register one or more handlers with a receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, IMessageHandlerRegistration<ICommandHandler> registrationSource);
    }
}