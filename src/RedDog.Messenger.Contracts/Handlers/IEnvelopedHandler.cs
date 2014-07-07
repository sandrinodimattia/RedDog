namespace RedDog.Messenger.Contracts.Handlers
{
    public interface IEnvelopedHandler
    {
        IEnvelope Envelope
        {
            get;
            set;
        }
    }
}