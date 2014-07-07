using System;
using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus.Registration
{
    public interface IMessageRegistration<TMessage>
        where TMessage : IMessage
    {
        Type[] GetTypes();
    }
}