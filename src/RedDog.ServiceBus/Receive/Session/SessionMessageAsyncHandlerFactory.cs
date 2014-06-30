using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive.Session
{
    internal class SessionMessageAsyncHandlerFactory: IMessageSessionAsyncHandlerFactory
    {
        private readonly string _ns;
        
        private readonly string _path;

        private readonly OnSessionMessage _messageHandler;

        private readonly OnSessionMessageOptions _options;

        /// <summary>
        /// Initialize the handler factory.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="path"></param>
        /// <param name="messageHandler"></param>
        /// <param name="options"></param>
        public SessionMessageAsyncHandlerFactory(string ns, string path, OnSessionMessage messageHandler, OnSessionMessageOptions options)
        {
            _ns = ns;
            _path = path;
            _messageHandler = messageHandler;
            _options = options;
        }

        /// <summary>
        /// Create a new session handler.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IMessageSessionAsyncHandler CreateInstance(MessageSession session, BrokeredMessage message)
        {
            ServiceBusEventSource.Log.CreateMessageSessionAsyncHandler(_ns, _path, session.SessionId, message.MessageId, message.CorrelationId);

            // Use the current handler.
            return new SessionMessageAsyncHandler(_ns, _path, session, _messageHandler, _options);
        }

        /// <summary>
        /// Dispose the session handler.
        /// </summary>
        /// <param name="handler"></param>
        public void DisposeInstance(IMessageSessionAsyncHandler handler)
        {
            var sessionHandler = handler as SessionMessageAsyncHandler;
            if (sessionHandler != null)
            {
                ServiceBusEventSource.Log.DisposeMessageSessionAsyncHandler(_ns, _path, sessionHandler.Session.SessionId);
            }
        }
    }
}