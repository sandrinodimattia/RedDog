using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive.Session
{
    public static class SessionMessageReceiverExtensions
    {
        public static Task StartAsync(this ISessionMessagePump pump, OnSessionMessage messageHandler)
        {
            return pump.StartAsync(messageHandler, null, null);
        }

        public static Task StartAsync(this ISessionMessagePump pump, OnSessionMessage messageHandler, OnSessionMessageOptions options)
        {
            return pump.StartAsync(messageHandler, null, options);
        }

        public static Task StartAsync(this ISessionMessagePump pump, OnSessionMessage messageHandler, OnSessionMessageException exception)
        {
            return pump.StartAsync(messageHandler, exception, null);
        }
    }
}