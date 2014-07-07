using System;
using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus.Registration
{
    public class ArrayMessageRegistration<TMessage> : IMessageRegistration<TMessage>
        where TMessage : IMessage
    {
        private readonly Type[] _types;

        public ArrayMessageRegistration(params Type[] types)
        {
            _types = types;
        }

        public Type[] GetTypes()
        {
            return _types;
        }
    }
}