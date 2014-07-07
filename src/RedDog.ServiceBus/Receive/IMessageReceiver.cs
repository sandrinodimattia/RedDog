using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive
{
    public interface IMessageReceiver
    {
        string Path
        {
            get;
        }

        ReceiveMode Mode
        {
            get;
        }
    }
}