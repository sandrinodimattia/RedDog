using System;

using RedDog.Messenger.Contracts.Handlers;

namespace RedDog.Messenger.Processor.Registration
{
    internal class TypeMessageHandlerRegistration<TMessageHandler> : MessageHandlerRegistration<TMessageHandler>
        where TMessageHandler : IMessageHandler
    {
        public TypeMessageHandlerRegistration(Type handlerInterface, Type handlerType)
            : base(handlerInterface)
        {
            RegisterMessageTypes(handlerType);
        }
    }
}