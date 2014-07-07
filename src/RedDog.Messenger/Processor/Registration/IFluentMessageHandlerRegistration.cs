using RedDog.Messenger.Contracts.Handlers;

namespace RedDog.Messenger.Processor.Registration
{
    public interface IFluentMessageHandlerRegistration<TMessageHandler> : IMessageHandlerRegistration<TMessageHandler>
        where TMessageHandler : IMessageHandler
    {
        IFluentMessageHandlerRegistration<TMessageHandler> With<TMessageHandlerType>()
            where TMessageHandlerType : TMessageHandler;
    }
}