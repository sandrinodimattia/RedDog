using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Send
{
    public class TopicMessageSender : IMessageSender
    {
        private readonly TopicClient _client;

        private readonly string _ns;

        public string Path
        {
            get { return _client.Path; }
        }

        public TopicMessageSender(TopicClient client)
        {
            _ns = client.MessagingFactory.GetShortNamespaceName();
            _client = client;
        }

        public Task SendAsync(BrokeredMessage message)
        {
            ServiceBusEventSource.Log.SendToTopic(_ns, _client.Path, message.SessionId, message.MessageId, message.CorrelationId);

            return _client.SendAsync(message);
        }

        public Task SendBatchAsync(BrokeredMessage[] messages)
        {
            foreach (var message in messages)
            {
                ServiceBusEventSource.Log.SendToTopic(_ns, _client.Path, message.SessionId, message.MessageId, message.CorrelationId);
            }

            return _client.SendBatchAsync(messages);
        }
    }
}