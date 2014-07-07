using RedDog.ServiceBus.Receive;

namespace RedDog.Messenger.Configuration
{
    public interface IMessagingReceiverConfiguration<out TConfiguration> : IMessagingConfiguration
        where TConfiguration : IMessagingConfiguration
    {
        TConfiguration WithErrorHandler(OnMessageException handler);
    }
}