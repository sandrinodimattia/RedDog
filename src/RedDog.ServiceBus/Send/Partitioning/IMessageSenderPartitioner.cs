namespace RedDog.ServiceBus.Send.Partitioning
{
    public interface IMessageSenderPartitioner
    {
        IMessageSender GetSender(IMessageSender[] senders);
    }
}