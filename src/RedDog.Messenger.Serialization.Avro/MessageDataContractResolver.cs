using System;
using System.Collections.Generic;
using Microsoft.Hadoop.Avro;
using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Serialization.Avro
{
    internal class MessageDataContractResolver : AvroDataContractResolver
    {
        private readonly IEnumerable<Type> _types;

        public MessageDataContractResolver(IEnumerable<Type> types)
        {
            _types = types;
        }

        public override IEnumerable<Type> GetKnownTypes(Type type)
        {
            if (type == typeof(IMessage))
                return _types;
            return base.GetKnownTypes(type);
        }
    }
}