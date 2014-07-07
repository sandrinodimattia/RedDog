using System;

using RedDog.Messenger.Contracts.Handlers;

namespace RedDog.Messenger.Processor.Registration
{
    internal class ArrayMessageHandlerRegistration<TMessageHandler> : MessageHandlerRegistration<TMessageHandler>
        where TMessageHandler : IMessageHandler
    {
        public ArrayMessageHandlerRegistration(Type handlerInterface, params Type[] handlerTypes)
            : base(handlerInterface)
        {
            foreach (var handlerType in handlerTypes)
            {
                RegisterMessageTypes(handlerType);
            }
        }
    }
}