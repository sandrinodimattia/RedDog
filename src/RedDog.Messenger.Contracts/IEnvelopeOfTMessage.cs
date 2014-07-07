namespace RedDog.Messenger.Contracts
{
    public interface IEnvelope<out TMessage> : IEnvelope
        where TMessage : IMessage
    {
        TMessage Body
        {
            get;
        }
    }
}