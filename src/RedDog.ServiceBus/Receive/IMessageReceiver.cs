using System.Threading.Tasks;
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

        Task StartAsync(OnMessage messageHandler, OnMessageException exceptionHandler, OnMessageOptions options);

        Task StopAsync();
    }
}