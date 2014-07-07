using System;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Bus.Registration
{
    public class TypeMessageRegistration<TMessage> : IMessageRegistration<TMessage>
        where TMessage : IMessage
    {
        private readonly Type _type;

        public TypeMessageRegistration(Type type)
        {
            _type = type;
        }

        public Type[] GetTypes()
        {
            return new[] {_type};
        }
    }
}