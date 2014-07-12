using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive.Session
{
    public class QueueSessionMessagePump : EventDrivenSessionMessagePump
    {
        private readonly QueueClient _client;

        public QueueSessionMessagePump(QueueClient client, OnSessionMessageOptions options = null)
            : base(client, client.Mode, client.MessagingFactory.GetShortNamespaceName(), client.Path, options)
        {
            _client = client;
        }

        /// <summary>
        /// Start the handler.
        /// </summary>
        /// <param name="sessionMessageAsyncHandlerFactory"></param>
        /// <param name="sessionHandlerOptions"></param>
        /// <returns></returns>
        internal override Task OnStartAsync(SessionMessageAsyncHandlerFactory sessionMessageAsyncHandlerFactory, SessionHandlerOptions sessionHandlerOptions)
        {
            return _client.RegisterSessionHandlerFactoryAsync(sessionMessageAsyncHandlerFactory, sessionHandlerOptions);
        }
    }
}