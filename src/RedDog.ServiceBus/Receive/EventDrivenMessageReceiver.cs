using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive
{
    public abstract class EventDrivenMessageReceiver : IMessageReceiver
    {
        private bool _initialized;

        private readonly MessageClientEntity _messageClient;

        private readonly ReceiveMode _mode;

        private readonly string _namespace;

        private readonly string _path;

        private readonly object _initializationLock = new object();

        public string Path
        {
            get { return _path; }
        }

        public string Namespace
        {
            get { return _namespace; }
        }
        public ReceiveMode Mode
        {
            get { return _mode; }
        }

        protected EventDrivenMessageReceiver(MessageClientEntity messageClient, ReceiveMode mode, string @namespace, string path)
        {
            _messageClient = messageClient;
            _mode = mode;
            _namespace = @namespace;
            _path = path;
        }

        public Task StartAsync(OnMessage messageHandler, OnMessageException exceptionHandler, OnMessageOptions options)
        {
            lock (_initializationLock)
            {
                if (_initialized)
                {
                    throw new MessageReceiverException("Message receiver has already been initialized.");
                }

                // Log.
                ServiceBusEventSource.Log.StartMessageReceiver(GetType().Name, Namespace, Path);

                // Initialize the handler options.
                var messageOptions = new Microsoft.ServiceBus.Messaging.OnMessageOptions();
                messageOptions.AutoComplete = options.AutoComplete;
                messageOptions.AutoRenewTimeout = options.AutoRenewTimeout;
                messageOptions.MaxConcurrentCalls = options.MaxConcurrentCalls;
                messageOptions.ExceptionReceived += (s, e) => 
                {
                    // Log.
                    ServiceBusEventSource.Log.MessageReceiverException(Namespace, Path, 
                        null, null, e.Action, e.Exception.Message, e.Exception.StackTrace);
                    
                    // Handle exception.
                    if (exceptionHandler != null)
                        exceptionHandler(e.Action, e.Exception);
                };

                // Mark receiver as initialized.
                _initialized = true;

                // Start.
                return OnStartAsync(messageHandler, messageOptions);
            }
        }

        internal abstract Task OnStartAsync(OnMessage messageHandler, Microsoft.ServiceBus.Messaging.OnMessageOptions messageOptions);

        public Task StopAsync()
        {
            return _messageClient.CloseAsync();
        }
    }
}