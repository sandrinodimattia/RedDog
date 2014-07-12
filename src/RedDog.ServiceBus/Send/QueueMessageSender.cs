using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Send
{
    public class QueueMessageSender : IMessageSender
    {
        private readonly QueueClient _client;

        private readonly string _ns;

        public string Path
        {
            get { return _client.Path; }
        }

        public QueueMessageSender(QueueClient client)
        {
            _client = client;
            _ns = client.MessagingFactory.GetShortNamespaceName();
        }

        public async Task SendAsync(BrokeredMessage message)
        {
            ServiceBusEventSource.Log.SendMessage(_ns, _client.Path, message.MessageId, message.CorrelationId, message.SessionId);

            var sendingAt = DateTime.Now;
            await _client.SendAsync(message).ConfigureAwait(false);

            ServiceBusEventSource.Log.SentMessage(_ns, _client.Path, message.MessageId, message.CorrelationId, message.SessionId, (DateTime.Now - sendingAt).TotalSeconds);
        }

        public async Task SendBatchAsync(BrokeredMessage[] messages)
        {
            ServiceBusEventSource.Log.SendMessageBatch(_ns, _client.Path, messages.Length);

            var sendingAt = DateTime.Now;
            await _client.SendBatchAsync(messages).ConfigureAwait(false);

            ServiceBusEventSource.Log.SentMessageBatch(_ns, _client.Path, messages.Length, (DateTime.Now - sendingAt).TotalSeconds);
        }
    }
}