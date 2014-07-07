using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Hadoop.Avro;
using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Serialization.Avro
{
    public class AvroSerializer : ISerializer
    {
        private readonly IEnumerable<Type> _messageTypes;

        private readonly AvroSerializerSettings _settings;

        public AvroSerializer()
        {
            _messageTypes = GetMessageTypes();
            _settings = new AvroSerializerSettings();
            _settings.Resolver = new MessageDataContractResolver(_messageTypes);
        }

        private IEnumerable<Type> GetMessageTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IMessage).IsAssignableFrom(t));
        }

        public Task<byte[]> Serialize<TData>(TData data)
            where TData : class
        {
            using (var buffer = new MemoryStream())
            {
                // Serialize the data.
                var avroSerializer = Microsoft.Hadoop.Avro.AvroSerializer.Create<TData>(_settings);
                avroSerializer.Serialize(buffer, data);

                // Return the contents of the buffer.
                buffer.Seek(0, SeekOrigin.Begin);
                return Task.FromResult(buffer.ToArray());
            }
        }

        public Task<TData> Deserialize<TData>(byte[] serializedMessage)
            where TData : class
        {
            using (var ms = new MemoryStream(serializedMessage))
            {
                var avroSerializer = Microsoft.Hadoop.Avro.AvroSerializer.Create<TData>(_settings);
                return Task.FromResult(avroSerializer.Deserialize(ms));
            }
        }
    }
}