using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus.Registration
{
    public interface IFluentMessageRegistration<TMessage> : IMessageRegistration<TMessage>
        where TMessage : IMessage
    {
        IFluentMessageRegistration<TMessage> With<TMessageType>()
            where TMessageType : TMessage;
    }
}