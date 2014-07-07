using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive
{
    public interface IMessagePump : IMessageReceiver
    {
        Task StartAsync(OnMessage messageHandler, OnMessageException exceptionHandler, OnMessageOptions options);

        Task StopAsync();
    }
}