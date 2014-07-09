using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive
{
    public class QueueMessagePump : EventDrivenMessagePump
    {
        private readonly QueueClient _client;

        public QueueMessagePump(QueueClient client)
            : base(client, client.Mode, client.MessagingFactory.GetShortNamespaceName(), client.Path)
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
            try
            {
                ServiceBusEventSource.Log.MessageReceived(Namespace, Path, message.MessageId, message.CorrelationId);

                // Handle the message.
                await messageHandler(message)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                ServiceBusEventSource.Log.MessageReceiverException(Namespace, Path, message.MessageId, message.CorrelationId, "OnMessage", exception.Message, exception.StackTrace);
                
                throw;
            }
        }
    }
}