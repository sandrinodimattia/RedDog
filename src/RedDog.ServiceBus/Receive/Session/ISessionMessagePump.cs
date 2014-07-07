using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive.Session
{
    public interface ISessionMessagePump : IMessageReceiver
    {
        Task StartAsync(OnSessionMessage messageHandler, OnSessionMessageException exceptionHandler, OnSessionMessageOptions options);

        Task StopAsync();
    }
}