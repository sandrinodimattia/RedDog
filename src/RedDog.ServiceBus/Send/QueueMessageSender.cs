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

        public Task SendAsync(BrokeredMessage message)
        {
            ServiceBusEventSource.Log.SendToQueue(_ns, _client.Path, message.SessionId, message.MessageId, message.CorrelationId);

            return _client.SendAsync(message);
        }

        public Task SendBatchAsync(BrokeredMessage[] messages)
        {
            foreach (var message in messages)
            {
                ServiceBusEventSource.Log.SendToQueue(_ns, _client.Path, message.SessionId, message.MessageId, message.CorrelationId);
            }

            return _client.SendBatchAsync(messages);
        }
    }
}