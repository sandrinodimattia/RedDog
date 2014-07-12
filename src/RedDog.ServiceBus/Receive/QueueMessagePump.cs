using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive
{
    public class QueueMessagePump : EventDrivenMessagePump
    {
        private readonly QueueClient _client;

        public QueueMessagePump(QueueClient client, OnMessageOptions options = null)
            : base(client, client.Mode, client.MessagingFactory.GetShortNamespaceName(), client.Path, options)
        {
            _client = client;
        }

        /// <summary>
        /// Start the handler.
        /// </summary>
        /// <returns></returns>
        internal override Task OnStartAsync(OnMessage messageHandler, Microsoft.ServiceBus.Messaging.OnMessageOptions messageOptions)
        {
            _client.OnMessageAsync(msg => HandleMessage(messageHandler, msg), messageOptions);

            return Task.FromResult(false);
        }

        /// <summary>
        /// Handle the message.
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task HandleMessage(OnMessage messageHandler, BrokeredMessage message)
        {
            ServiceBusEventSource.Log.MessageReceived(Namespace, Path, message.MessageId, message.CorrelationId, message.DeliveryCount, message.Size);

            // Handle the message.
            await messageHandler(message)
                .ConfigureAwait(false);
        }
    }
}