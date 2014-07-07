using RedDog.Messenger.Configuration;

namespace RedDog.Messenger.Filters.Serialization
{
    public static class GzipCompressionConfigurationExtensions
    {
        public static TConfiguration WithGzipCompression<TConfiguration>(this TConfiguration configuration)
            where TConfiguration : IMessagingConfiguration, IMessagingSerializerConfiguration<TConfiguration>
        {
            return configuration.WithSerializationFilter(new GzipCompressionMessageFilter());
        }
    }
}
