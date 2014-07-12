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

        private readonly OnSessionMessageOptions _defaultOptions;

        protected EventDrivenSessionMessagePump(MessageClientEntity messageClient, ReceiveMode mode, string @namespace, string path, OnSessionMessageOptions defaultOptions)
            : base(mode, @namespace, path)
        {
            _messageClient = messageClient;
            _defaultOptions = defaultOptions ?? new OnSessionMessageOptions();
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
                    options = _defaultOptions;

                // Log.
                ServiceBusEventSource.Log.SessionMessagePumpStart(GetType().Name, Namespace, Path, options.AutoRenewSessionTimeout, options.MaxConcurrentSessions);

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
                        ServiceBusEventSource.Log.MessagePumpExceptionReceived(Namespace, Path,
                            e.Action, e.Exception);

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