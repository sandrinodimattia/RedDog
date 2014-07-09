using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus
{
    public static class MessagingFactorySubscriptionExtensions
    {
        public static Task<SubscriptionClient> EnsureSubscriptionAsync(this MessagingFactory factory, string topicPath, string subscriptionName, bool requiresSession = false, ReceiveMode mode = ReceiveMode.PeekLock)
        {
            return EnsureSubscriptionAsync(factory, new SubscriptionDescription(topicPath, subscriptionName) { RequiresSession = requiresSession }, mode);
        }

        public async static Task<SubscriptionClient> EnsureSubscriptionAsync(this MessagingFactory factory, SubscriptionDescription subscriptionDescription, ReceiveMode mode = ReceiveMode.PeekLock)
        {
            await new NamespaceManager(factory.Address, factory.GetSettings().TokenProvider)
                .TryCreateEntity(
                    mgr => SubscriptionCreateAsync(mgr, subscriptionDescription),
                    mgr => SubscriptionShouldExistAsync(mgr, subscriptionDescription)).ConfigureAwait(false);

            return factory.CreateSubscriptionClient(subscriptionDescription.TopicPath, subscriptionDescription.Name, mode);
        }

        private async static Task SubscriptionCreateAsync(NamespaceManager ns, SubscriptionDescription subscriptionDescription)
        {
            if (!await ns.SubscriptionExistsAsync(subscriptionDescription.TopicPath, subscriptionDescription.Name).ConfigureAwait(false))
            {
                await ns.CreateSubscriptionAsync(subscriptionDescription).ConfigureAwait(false);

                ServiceBusEventSource.Log.CreatedSubscription(ns.Address.ToString(), subscriptionDescription.TopicPath, subscriptionDescription.Name);
            }
        }

        private async static Task SubscriptionShouldExistAsync(NamespaceManager ns, SubscriptionDescription subscriptionDescription)
        {
            if (!await ns.SubscriptionExistsAsync(subscriptionDescription.TopicPath, subscriptionDescription.Name).ConfigureAwait(false))
            {
                throw new MessagingEntityNotFoundException("Subscription: " + subscriptionDescription.TopicPath + "/" + subscriptionDescription.Name);
            }
        }
    }
}