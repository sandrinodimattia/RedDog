using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive.Session
{
    internal class SessionMessageAsyncHandlerFactory: IMessageSessionAsyncHandlerFactory
    {
        private readonly string _receiverNamespace;
        
        private readonly string _receiverPath;

        private readonly OnSessionMessage _messageHandler;

        private readonly OnSessionMessageOptions _options;

        /// <summary>
        /// Initialize the handler factory.
        /// </summary>
        /// <param name="receiverNamespace"></param>
        /// <param name="receiverPath"></param>
        /// <param name="messageHandler"></param>
        /// <param name="options"></param>
        public SessionMessageAsyncHandlerFactory(string receiverNamespace, string receiverPath, OnSessionMessage messageHandler, OnSessionMessageOptions options)
        {
            _receiverNamespace = receiverNamespace;
            _receiverPath = receiverPath;
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
            ServiceBusEventSource.Log.SessonAccepted(_receiverNamespace, _receiverPath, session.SessionId, message.MessageId, message.CorrelationId);

            // Use the current handler.
            return new SessionMessageAsyncHandler(_receiverNamespace, _receiverPath, session, _messageHandler, _options);
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
                ServiceBusEventSource.Log.SessionHandlerDisposed(_receiverNamespace, _receiverPath, sessionHandler.Session.SessionId);
            }
        }
    }
}