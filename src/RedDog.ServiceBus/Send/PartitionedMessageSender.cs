using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Send.Partitioning;

namespace RedDog.ServiceBus.Send
{
    public class PartitionedMessageSender : IMessageSender
    {
        private readonly string _path;

        private readonly IMessageSender[] _senders;

        private readonly IMessageSenderPartitioner _partitioner;

        public string Path
        {
            get { return _path; }
        }

        public PartitionedMessageSender(string displayPath, IMessageSender[] senders)
            : this(displayPath, senders, new RoundRobinMessageSenderPartitioner())
        {
            
        }
        
        public PartitionedMessageSender(string displayPath, IMessageSender[] senders, IMessageSenderPartitioner partitioner)
        {
            _path = displayPath;
            _senders = senders;
            _partitioner = partitioner;
        }

        public Task SendAsync(BrokeredMessage message)
        {
            return _partitioner.GetSender(_senders)
                .SendAsync(message);
        }

        public Task SendBatchAsync(BrokeredMessage[] messages)
        {
            return _partitioner.GetSender(_senders)
                .SendBatchAsync(messages);
        }
    }
}