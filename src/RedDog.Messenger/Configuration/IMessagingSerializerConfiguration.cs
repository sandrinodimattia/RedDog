using RedDog.Messenger.Filters;
using RedDog.Messenger.Serialization;

namespace RedDog.Messenger.Configuration
{
    public interface IMessagingSerializerConfiguration<out TConfiguration> : IMessagingConfiguration
        where TConfiguration : IMessagingConfiguration
    {
        TConfiguration WithSerializer(ISerializer serializer);

        TConfiguration WithSerializationFilter(IMessageFilter filter);
    }
}