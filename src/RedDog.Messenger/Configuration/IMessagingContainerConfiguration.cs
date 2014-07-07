using RedDog.Messenger.Composition;

namespace RedDog.Messenger.Configuration
{
    public interface IMessagingContainerConfiguration<out TConfiguration> : IMessagingConfiguration
        where TConfiguration : IMessagingConfiguration
    {
        TConfiguration WithContainer(IContainer container);
    }
}