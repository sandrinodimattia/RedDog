namespace RedDog.ServiceBus.Send.Partitioning
{
    public class RoundRobinMessageSenderPartitioner : IMessageSenderPartitioner
    {
        private int _currentSender = -1;

        private readonly object _sendersLock = new object();

        public IMessageSender GetSender(IMessageSender[] senders)
        {
            lock (_sendersLock)
            {
                _currentSender = ++_currentSender % senders.Length;
                return senders[_currentSender];
            }
        }
    }
}
