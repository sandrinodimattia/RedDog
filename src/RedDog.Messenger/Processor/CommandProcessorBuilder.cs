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

        public ICommandProcessorConfiguration RegisterCommandHandler<TCommandHandler>(IMessagePump receiver)
            where TCommandHandler : ICommandHandler
        {
            RegisterCommandHandlers(receiver, new TypeMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), typeof(TCommandHandler)));

            // Continue.
            return this;
        }

        public ICommandProcessorConfiguration RegisterCommandHandler<TCommandHandler>(ISessionMessagePump receiver)
            where TCommandHandler : ICommandHandler
        {
            RegisterCommandHandlers(receiver, new TypeMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), typeof(TCommandHandler)));

            // Continue.
            return this;
        }

        public ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, Action<IFluentMessageHandlerRegistration<ICommandHandler>> config)
        {
            // Create configuration object and call external configuration.
            var handlers = new FluentMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>));
            config(handlers);

            // Register handlers.
            RegisterCommandHandlers(receiver, handlers);

            // Continue.
            return this;
        }

        public ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, Action<IFluentMessageHandlerRegistration<ICommandHandler>> config)
        {
            // Create configuration object and call external configuration.
            var handlers = new FluentMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>));
            config(handlers);
            
            // Register handlers.
            RegisterCommandHandlers(receiver, handlers);

            // Continue.
            return this;
        }

        public ICommandProcessorConfiguration RegisterCommandHandlers(IMessagePump receiver, params Type[] types)
        {
            RegisterCommandHandlers(receiver, new ArrayMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), types));

            // Continue.
            return this;
        }

        public ICommandProcessorConfiguration RegisterCommandHandlers(ISessionMessagePump receiver, params Type[] types)
        {
            RegisterCommandHandlers(receiver, new ArrayMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), types));

            // Continue.
            return this;
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