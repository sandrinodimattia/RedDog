using System;
using System.Linq;

using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;

namespace RedDog.Messenger.Processor
{
    public class EventProcessorBuilder : ProcessorBuilder<IEventProcessorConfiguration>, IEventProcessorConfiguration
    {
        public IEventProcessor Build()
        {
            return new EventProcessor(this);
        }

        public static IEventProcessorConfiguration Create()
        {
            return new EventProcessorBuilder { AllowMultipleMessageHandlers = true };
        }

        public IEventProcessorConfiguration RegisterEventHandler<TEventHandler>(IMessagePump receiver)
            where TEventHandler : IEventHandler
        {
            RegisterEventHandlers(receiver, new TypeMessageHandlerRegistration<IEventHandler>(typeof(IEventHandler<>), typeof(TEventHandler)));

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandler<TEventHandler>(ISessionMessagePump receiver)
            where TEventHandler : IEventHandler
        {
            RegisterEventHandlers(receiver, new TypeMessageHandlerRegistration<IEventHandler>(typeof(IEventHandler<>), typeof(TEventHandler)));

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandlers(IMessagePump receiver, Action<IFluentMessageHandlerRegistration<IEventHandler>> config)
        {
            // Create configuration object and call external configuration.
            var handlers = new FluentMessageHandlerRegistration<IEventHandler>(typeof(IEventHandler<>));
            config(handlers);

            // Register handlers.
            RegisterEventHandlers(receiver, handlers);

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandlers(ISessionMessagePump receiver, Action<IFluentMessageHandlerRegistration<IEventHandler>> config)
        {
            // Create configuration object and call external configuration.
            var handlers = new FluentMessageHandlerRegistration<IEventHandler>(typeof(IEventHandler<>));
            config(handlers);
            
            // Register handlers.
            RegisterEventHandlers(receiver, handlers);

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandlers(IMessagePump receiver, params Type[] types)
        {
            RegisterEventHandlers(receiver, new ArrayMessageHandlerRegistration<IEventHandler>(typeof(IEventHandler<>), types));

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandlers(ISessionMessagePump receiver, params Type[] types)
        {
            RegisterEventHandlers(receiver, new ArrayMessageHandlerRegistration<IEventHandler>(typeof(IEventHandler<>), types));

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandlers(IMessagePump receiver, IMessageHandlerRegistration<IEventHandler> registrationSource)
        {
            foreach (var registration in registrationSource.GetRegistrations())
            {
                AddMessageType(receiver, registration.Key, registration.Value.ToArray());
            }

            // Continue.
            return this;
        }

        public IEventProcessorConfiguration RegisterEventHandlers(ISessionMessagePump receiver, IMessageHandlerRegistration<IEventHandler> registrationSource)
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