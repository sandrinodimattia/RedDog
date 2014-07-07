using System.Threading.Tasks;

namespace RedDog.ServiceBus.Receive
{
    public static class MessageReceiverExtensions
    {
        public static Task StartAsync(this IMessagePump pump, OnMessage messageHandler)
        {
            return pump.StartAsync(messageHandler, null, null);
        }

        public static Task StartAsync(this IMessagePump pump, OnMessage messageHandler, OnMessageOptions options)
        {
            return pump.StartAsync(messageHandler, null, options);
        }

        public static Task StartAsync(this IMessagePump pump, OnMessage messageHandler, OnMessageException exception)
        {
            return pump.StartAsync(messageHandler, exception, null);
        }
    }
}