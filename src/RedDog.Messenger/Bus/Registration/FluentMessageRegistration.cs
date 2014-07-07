using System;
using System.Collections.Concurrent;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus.Registration
{
    public class FluentMessageRegistration<TMessage> : IMessageRegistration<TMessage>, IFluentMessageRegistration<TMessage>
        where TMessage : IMessage
    {
        private readonly ConcurrentBag<Type> _messageTypes;

        public FluentMessageRegistration()
        {
            _messageTypes = new ConcurrentBag<Type>();
        }

        public IFluentMessageRegistration<TMessage> With<TMessageType>()
            where TMessageType : TMessage
        {
            _messageTypes.Add(typeof(TMessageType));
            return this;
        }

        public Type[] GetTypes()
        {
            return _messageTypes.ToArray();
        }
    }
}