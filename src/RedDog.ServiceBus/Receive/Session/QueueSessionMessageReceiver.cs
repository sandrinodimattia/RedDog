using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive.Session
{
    public class QueueSessionMessageReceiver : EventDrivenSessionMessageReceiver
    {
        private readonly QueueClient _client;

        public QueueSessionMessageReceiver(QueueClient client)
            : base(client, client.MessagingFactory.GetShortNamespaceName(), client.Path)
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