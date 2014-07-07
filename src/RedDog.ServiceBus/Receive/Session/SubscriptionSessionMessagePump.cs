using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive.Session
{
    public class SubscriptionSessionMessagePump : EventDrivenSessionMessagePump
    {
        private readonly SubscriptionClient _client;

        public SubscriptionSessionMessagePump(SubscriptionClient client)
            : base(client, client.Mode, client.MessagingFactory.GetShortNamespaceName(), client.TopicPath + "/" + client.Name)
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