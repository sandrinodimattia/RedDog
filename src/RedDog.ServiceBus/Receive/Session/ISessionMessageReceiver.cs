using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive.Session
{
    public interface ISessionMessageReceiver
    {
        Task StartAsync(OnSessionMessage messageHandler, OnSessionMessageException exceptionHandler, OnSessionMessageOptions options);

        Task StopAsync();
    }
}