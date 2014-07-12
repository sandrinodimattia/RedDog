using System;

using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;

namespace RedDog.Messenger.Processor
{
    public static class ConfigurationExtensions
    {
        public static ICommandProcessorConfiguration RegisterCommandHandler<TCommandHandler>(this ICommandProcessorConfiguration configuration, IMessagePump receiver)
            where TCommandHandler : ICommandHandler
        {
            configuration.RegisterCommandHandlers(receiver, new TypeMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), typeof(TCommandHandler)));

            // Continue.
            return configuration;
        }

        public static ICommandProcessorConfiguration RegisterCommandHandler<TCommandHandler>(this ICommandProcessorConfiguration configuration, ISessionMessagePump receiver)
            where TCommandHandler : ICommandHandler
        {
            configuration.RegisterCommandHandlers(receiver, new TypeMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), typeof(TCommandHandler)));

            // Continue.
            return configuration;
        }

        public static ICommandProcessorConfiguration RegisterCommandHandlers(this ICommandProcessorConfiguration configuration, IMessagePump receiver, Action<IFluentMessageHandlerRegistration<ICommandHandler>> config)
        {
            // Create configuration object and call external configuration.
            var handlers = new FluentMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>));
            config(handlers);

            // Register handlers.
            configuration.RegisterCommandHandlers(receiver, handlers);

            // Continue.
            return configuration;
        }

        public static ICommandProcessorConfiguration RegisterCommandHandlers(this ICommandProcessorConfiguration configuration, ISessionMessagePump receiver, Action<IFluentMessageHandlerRegistration<ICommandHandler>> config)
        {
            // Create configuration object and call external configuration.
            var handlers = new FluentMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>));
            config(handlers);

            // Register handlers.
            configuration.RegisterCommandHandlers(receiver, handlers);

            // Continue.
            return configuration;
        }

        public static ICommandProcessorConfiguration RegisterCommandHandlers(this ICommandProcessorConfiguration configuration, IMessagePump receiver, params Type[] types)
        {
            configuration.RegisterCommandHandlers(receiver, new ArrayMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), types));

            // Continue.
            return configuration;
        }

        public static ICommandProcessorConfiguration RegisterCommandHandlers(this ICommandProcessorConfiguration configuration, ISessionMessagePump receiver, params Type[] types)
        {
            configuration.RegisterCommandHandlers(receiver, new ArrayMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), types));

            // Continue.
            return configuration;
        }
    }
}
