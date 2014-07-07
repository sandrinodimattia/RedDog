using RedDog.Messenger.Configuration;

namespace RedDog.Messenger.Serialization.Avro
{
    public static class AvroSerializerConfigurationExtensions
    {
        public static TConfiguration WithAvroSerialization<TConfiguration>(this TConfiguration configuration)
            where TConfiguration : IMessagingSerializerConfiguration<TConfiguration>
        {
            return configuration.WithSerializer(new AvroSerializer());
        }
    }
}
