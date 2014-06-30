using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive.Session
{
    public abstract class EventDrivenSessionMessageReceiver : ISessionMessageReceiver
    {
        private bool _initialized;

        private readonly MessageClientEntity _messageClient;

        private readonly string _ns;

        private readonly string _path;

        private readonly object _initializationLock = new object();

        protected EventDrivenSessionMessageReceiver(MessageClientEntity messageClient, string ns, string path)
        {
            _messageClient = messageClient;
            _ns = ns;
            _path = path;
        }

        public Task StartAsync(OnSessionMessage messageHandler, OnSessionMessageException exceptionHandler, OnSessionMessageOptions options)
        {
            lock (_initializationLock)
            {
                if (_initialized)
                {
                    throw new MessageReceiverException("Message receiver has already been initialized.");
                }

                // Log.
                ServiceBusEventSource.Log.StartSessionMessageReceiver(GetType().Name, _ns, _path);

                // Initialize the handler options.
                var sessionHandlerOptions = new SessionHandlerOptions();
                sessionHandlerOptions.AutoComplete = options.AutoComplete;
                sessionHandlerOptions.AutoRenewTimeout = options.AutoRenewSessionTimeout;
                sessionHandlerOptions.MaxConcurrentSessions = options.MaxConcurrentSessions;
                sessionHandlerOptions.MessageWaitTimeout = options.MessageWaitTimeout;
                sessionHandlerOptions.ExceptionReceived += (s, e) => 
                {
                    // Log.
                    ServiceBusEventSource.Log.SessionMessageReceiverException(_ns, _path, 
                        null, null, null, e.Action, e.Exception.Message, e.Exception.StackTrace);
                    
                    // Handle exception.
                    if (exceptionHandler != null)
                        exceptionHandler(e.Action, e.Exception);
                };

                // Start.
                return OnStartAsync(new SessionMessageAsyncHandlerFactory(_ns, _path, messageHandler, options), new SessionHandlerOptions
                {
                    AutoComplete = options.AutoComplete,
                    AutoRenewTimeout = options.AutoRenewSessionTimeout,
                    MaxConcurrentSessions = options.MaxConcurrentSessions,
                    MessageWaitTimeout = options.MessageWaitTimeout
                });

                // Mark receiver as initialized.
                _initialized = true;
            }
        }

        internal abstract Task OnStartAsync(SessionMessageAsyncHandlerFactory sessionMessageAsyncHandlerFactory, SessionHandlerOptions sessionHandlerOptions);

        public Task StopAsync()
        {
            return _messageClient.CloseAsync();
        }
    }
}