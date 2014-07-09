using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus
{
    public static class MessagingFactoryTopicExtensions
    {
        public async static Task<TopicClient> EnsureTopicAsync(this MessagingFactory factory, TopicDescription topicDescription)
        {
            await new NamespaceManager(factory.Address, factory.GetSettings().TokenProvider)
                .TryCreateEntity(
                    mgr => TopicCreateAsync(mgr, topicDescription),
                    mgr => TopicShouldExistAsync(mgr, topicDescription)).ConfigureAwait(false);

            return factory.CreateTopicClient(topicDescription.Path);
        }

        private async static Task TopicCreateAsync(NamespaceManager ns, TopicDescription topicDescription)
        {
            if (!await ns.TopicExistsAsync(topicDescription.Path).ConfigureAwait(false))
            {
                await ns.CreateTopicAsync(topicDescription).ConfigureAwait(false);

                ServiceBusEventSource.Log.CreatedTopic(ns.Address.ToString(), topicDescription.Path);
            }
        }

        private async static Task TopicShouldExistAsync(NamespaceManager ns, TopicDescription topicDescription)
        {
            if (!await ns.TopicExistsAsync(topicDescription.Path).ConfigureAwait(false))
            {
                throw new MessagingEntityNotFoundException("Topic: " + topicDescription.Path);
            }
        }
    }
}