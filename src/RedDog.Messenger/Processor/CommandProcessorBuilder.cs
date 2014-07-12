using System;
using System.Linq;

using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;

namespace RedDog.Messenger.Processor
{
    public class CommandProcessorBuilder : ProcessorBuilder<ICommandProcessorConfiguration>, ICommandProcessorConfiguration
    {
        public ICommandProcessor Build()
        {
            return new CommandProcessor(this);
        }

        public static ICommandProcessorConfiguration Create()
        {
            return new CommandProcessorBuilder();
        }


        public ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, IMessageHandlerRegistration<ICommandHandler> registrationSource)
        {
            foreach (var registration in registrationSource.GetRegistrations())
            {
                AddMessageType(receiver, registration.Key, registration.Value.ToArray());
            }

            // Continue.
            return this;
        }

        public ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, IMessageHandlerRegistration<ICommandHandler> registrationSource)
        {
            foreach (var registration in registrationSource.GetRegistrations())
            {
                AddMessageType(receiver, registration.Key, registration.Value.ToArray());
            }

            // Continue.
            return this;
        }
    }
}