using RedDog.Messenger.Configuration;

namespace RedDog.Messenger.Serialization.Json
{
    public static class NewtonsoftJsonSerializerConfigurationExtensions
    {
        public static TConfiguration WithNewtonsoftJsonSerializer<TConfiguration>(this TConfiguration configuration)
            where TConfiguration : IMessagingSerializerConfiguration<TConfiguration>
        {
            return configuration.WithSerializer(new NewtonsoftJsonSerializer());
        }
    }
}
