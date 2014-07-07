using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive.Session
{
    public abstract class EventDrivenSessionMessagePump : MessagePump, ISessionMessagePump
    {
        private bool _initialized;

        private readonly object _initializationLock = new object();

        private readonly MessageClientEntity _messageClient;

        protected EventDrivenSessionMessagePump(MessageClientEntity messageClient, ReceiveMode mode, string @namespace, string path)
            : base(mode, @namespace, path)
        {
            _messageClient = messageClient;
        }

        public Task StartAsync(OnSessionMessage messageHandler, OnSessionMessageException exceptionHandler, OnSessionMessageOptions options)
        {
            lock (_initializationLock)
            {
                if (_initialized)
                    throw new MessageReceiverException("Message receiver has already been initialized.");

                if (messageHandler == null)
                    throw new ArgumentNullException("messageHandler");

                if (options == null)
                    options = new OnSessionMessageOptions();

                // Log.
                ServiceBusEventSource.Log.StartSessionMessageReceiver(GetType().Name, Namespace, Path);

                // Initialize the handler options.
                var sessionHandlerOptions = new SessionHandlerOptions();
                sessionHandlerOptions.AutoComplete = options.AutoComplete;
                sessionHandlerOptions.AutoRenewTimeout = options.AutoRenewSessionTimeout;
                sessionHandlerOptions.MaxConcurrentSessions = options.MaxConcurrentSessions;
                sessionHandlerOptions.MessageWaitTimeout = options.MessageWaitTimeout;
                sessionHandlerOptions.ExceptionReceived += (s, e) => 
                {
                    if (e.Exception != null)
                    {
                        // Log.
                        ServiceBusEventSource.Log.SessionMessageReceiverException(Namespace, Path,
                            null, null, null, e.Action, e.Exception.Message, e.Exception.StackTrace);

                        // Handle exception.
                        if (exceptionHandler != null)
                            exceptionHandler(e.Action, e.Exception);
                    }
                };

                // Mark receiver as initialized.
                _initialized = true;

                // Start.
                return OnStartAsync(new SessionMessageAsyncHandlerFactory(Namespace, Path, messageHandler, options), new SessionHandlerOptions
                {
                    AutoComplete = options.AutoComplete,
                    AutoRenewTimeout = options.AutoRenewSessionTimeout,
                    MaxConcurrentSessions = options.MaxConcurrentSessions,
                    MessageWaitTimeout = options.MessageWaitTimeout
                });
            }
        }

        internal abstract Task OnStartAsync(SessionMessageAsyncHandlerFactory sessionMessageAsyncHandlerFactory, SessionHandlerOptions sessionHandlerOptions);

        public Task StopAsync()
        {
            return _messageClient.CloseAsync();
        }
    }
}