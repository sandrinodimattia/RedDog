using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Diagnostics;

namespace RedDog.ServiceBus.Receive
{
    public abstract class EventDrivenMessagePump : MessagePump, IMessagePump
    {
        private bool _initialized;

        private readonly object _initializationLock = new object();

        private readonly MessageClientEntity _messageClient;

        private readonly OnMessageOptions _defaultOptions;

        protected EventDrivenMessagePump(MessageClientEntity messageClient, ReceiveMode mode, string @namespace, string path, OnMessageOptions defaultOptions)
            : base(mode, @namespace, path)
        {
            _messageClient = messageClient;
            _defaultOptions = defaultOptions ?? new OnMessageOptions();
        }

        public Task StartAsync(OnMessage messageHandler, OnMessageException exceptionHandler, OnMessageOptions options)
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
                ServiceBusEventSource.Log.MessagePumpStart(GetType().Name, Namespace, Path, options.AutoRenewTimeout, options.MaxConcurrentCalls);

                // Initialize the handler options.
                var messageOptions = new Microsoft.ServiceBus.Messaging.OnMessageOptions();
                messageOptions.AutoComplete = options.AutoComplete;
                messageOptions.AutoRenewTimeout = options.AutoRenewTimeout;
                messageOptions.MaxConcurrentCalls = options.MaxConcurrentCalls;
                messageOptions.ExceptionReceived += (s, e) => 
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