using Microsoft.ServiceBus.Messaging;

using Microsoft.WindowsAzure;

namespace RedDog.ServiceBus.Tests.Integration.TestUtils
{
    public static class ServiceBusEntityFactory
    {

        public static DisposableMessagingClient<QueueClient> DeleteAndCreateQueue(string name)
        {
            var factory = MessagingFactory.CreateFromConnectionString(CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString"));
            return new DisposableMessagingClient<QueueClient>(factory, factory.EnsureQueueAsync(new QueueDescription(name)).Result);
        }

        public static DisposableMessagingClient<TopicClient> DeleteAndCreateTopic(string name)
        {
            var factory = MessagingFactory.CreateFromConnectionString(CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString"));
            return new DisposableMessagingClient<TopicClient>(factory, factory.EnsureTopicAsync(new TopicDescription(name)).Result);
        }

        public static DisposableMessagingClient<SubscriptionClient> DeleteAndCreateTopicSubscription(string topic, string name)
        {
            var factory = MessagingFactory.CreateFromConnectionString(CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString"));
            return new DisposableMessagingClient<SubscriptionClient>(factory, factory.EnsureSubscriptionAsync(new SubscriptionDescription(topic, name)).Result);
        }
    }
}