using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive
{
    public static class MessageReceiverExtensions
    {
        public static Task StartAsync(this IMessageReceiver receiver, OnMessage messageHandler)
        {
            return receiver.StartAsync(messageHandler, null, null);
        }

        public static Task StartAsync(this IMessageReceiver receiver, OnMessage messageHandler, OnMessageOptions options)
        {
            return receiver.StartAsync(messageHandler, null, options);
        }

        public static Task StartAsync(this IMessageReceiver receiver, OnMessage messageHandler, OnMessageException exception)
        {
            return receiver.StartAsync(messageHandler, exception, null);
        }
    }
}