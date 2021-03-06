using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive.Session
{
    internal class SessionMessageAsyncHandler : IMessageSessionAsyncHandler
    {
        private readonly string _receiverNamespace;
        
        private readonly string _receiverPath;
        
        private readonly MessageSession _session;

        private readonly OnSessionMessage _messageHandler;

        private readonly OnSessionMessageOptions _options;

        public MessageSession Session
        {
            get { return _session; }
        }

        public SessionMessageAsyncHandler(string receiverNamespace, string receiverPath, MessageSession session, OnSessionMessage messageHandler, OnSessionMessageOptions options)
        {
            _receiverNamespace = receiverNamespace;
            _receiverPath = receiverPath;
            _session = session;
            _messageHandler = messageHandler;
            _options = options;
        }

        public async Task OnMessageAsync(MessageSession session, BrokeredMessage message)
        {
            try
            {
                ServiceBusEventSource.Log.SessionMessageReceived(_receiverNamespace, _receiverPath, message.MessageId, message.CorrelationId, message.SessionId, message.DeliveryCount, message.Size);

                // Handle the message.
                await _messageHandler(session, message)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                ServiceBusEventSource.Log.MessagePumpExceptionReceived(_receiverNamespace, _receiverPath, "OnSessionMessage", exception);

                // Don't allow other messages to be processed.
                if (_options.RequireSequentialProcessing)
                {
                    message.Abandon();
                    session.Abort();
                }

                throw;
            }
        }

        public Task OnCloseSessionAsync(MessageSession session)
        {
            ServiceBusEventSource.Log.SessionClosed(_receiverNamespace, _receiverPath, session.SessionId);

            // No need to do anything else.
            return Task.FromResult(0);
        }

        public Task OnSessionLostAsync(Exception exception)
        {
            ServiceBusEventSource.Log.SessionLost(_receiverNamespace, _receiverPath, _session.SessionId, exception);
            
            // No need to do anything else.
            return Task.FromResult(0);
        }
    }
}