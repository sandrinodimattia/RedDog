using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive.Session
{
    public static class SessionMessageReceiverExtensions
    {
        public static Task StartAsync(this ISessionMessageReceiver receiver, OnSessionMessage messageHandler)
        {
            return receiver.StartAsync(messageHandler, null, null);
        }

        public static Task StartAsync(this ISessionMessageReceiver receiver, OnSessionMessage messageHandler, OnSessionMessageOptions options)
        {
            return receiver.StartAsync(messageHandler, null, options);
        }

        public static Task StartAsync(this ISessionMessageReceiver receiver, OnSessionMessage messageHandler, OnSessionMessageException exception)
        {
            return receiver.StartAsync(messageHandler, exception, null);
        }
    }
}