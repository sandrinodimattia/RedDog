using System.Linq;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus
{
    public static class MessagingFactoryQueueExtensions
    {
        public static Task<QueueClient> EnsureQueueAsync(this MessagingFactory factory, string queueName, bool requiresSession = false, ReceiveMode mode = ReceiveMode.PeekLock)
        {
            return EnsureQueueAsync(factory, new QueueDescription(queueName) { RequiresSession = requiresSession }, mode);
        }

        public async static Task<QueueClient> EnsureQueueAsync(this MessagingFactory factory, QueueDescription queueDescription, ReceiveMode mode = ReceiveMode.PeekLock)
        {
            await new NamespaceManager(factory.Address, factory.GetSettings().TokenProvider)
                .TryCreateEntity(
                    mgr => QueueCreateAsync(mgr, queueDescription),
                    mgr => QueueShouldExistAsync(mgr, queueDescription));

            return factory.CreateQueueClient(queueDescription.Path, mode);
        }

        private async static Task QueueCreateAsync(NamespaceManager ns, QueueDescription queueDescription)
        {
            if (!await ns.QueueExistsAsync(queueDescription.Path))
            {
                await ns.CreateQueueAsync(queueDescription);

                ServiceBusEventSource.Log.CreatedQueue(ns.Address.ToString(), queueDescription.Path);
            }
        }

        private async static Task QueueShouldExistAsync(NamespaceManager ns, QueueDescription queueDescription)
        {
            if (!await ns.QueueExistsAsync(queueDescription.Path))
            {
                throw new MessagingEntityNotFoundException("Queue: " + queueDescription.Path);
            }
        }
    }
}