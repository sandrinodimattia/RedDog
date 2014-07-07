using System;

using RedDog.Messenger.Contracts.Handlers;

namespace RedDog.Messenger.Processor.Registration
{
    internal class FluentMessageHandlerRegistration<TMessageHandler> : MessageHandlerRegistration<TMessageHandler>, IFluentMessageHandlerRegistration<TMessageHandler>
        where TMessageHandler : IMessageHandler
    {
        public FluentMessageHandlerRegistration(Type handlerInterface)
            : base(handlerInterface)
        {
            
        }

        public IFluentMessageHandlerRegistration<TMessageHandler> With<TMessageHandlerType>()
            where TMessageHandlerType : TMessageHandler
        {
            RegisterMessageTypes(typeof(TMessageHandlerType));
            
            // Continue.
            return this;
        }
    }
}