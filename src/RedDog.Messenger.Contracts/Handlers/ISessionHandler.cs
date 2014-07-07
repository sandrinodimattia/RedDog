namespace RedDog.Messenger.Contracts.Handlers
{
    public interface ISessionHandler
    {
        ISession Session
        {
            get;
            set;
        }
    }
}