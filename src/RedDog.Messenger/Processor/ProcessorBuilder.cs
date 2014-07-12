using System;
using System.Collections.Generic;

using RedDog.Messenger.Configuration;
using RedDog.Messenger.Diagnostics;

using RedDog.ServiceBus.Receive;

namespace RedDog.Messenger.Processor
{
    public abstract class ProcessorBuilder<TConfiguration> : MessagingConfiguration<TConfiguration>, IProcessorConfiguration<TConfiguration>
        where TConfiguration : class, IProcessorConfiguration<TConfiguration>
    {
        private readonly Dictionary<object, MessageHandlerMap> _handlerMappings;

        private readonly object _mappingsLock = new object();

        /// <summary>
        /// Get a list of registered message types.
        /// </summary>
        public IReadOnlyDictionary<object, MessageHandlerMap> Receivers
        {
            get { return _handlerMappings; }
        }

        /// <summary>
        /// Allow
        /// </summary>
        public bool AllowMultipleMessageHandlers
        {
            get;
            set;
        }

        /// <summary>
        /// Error handler for custom logging etc..
        /// </summary>
        public OnMessageException ErrorHandler
        {
            get;
            set;
        }
        
        /// <summary>
        /// Initialize the builder.
        /// </summary>
        protected ProcessorBuilder()
        {
            _handlerMappings = new Dictionary<object, MessageHandlerMap>();
        }

        /// <summary>
        /// Find handlers for a receiver and a message type.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public IReadOnlyList<Type> GetHandlerTypes(object receiver, Type messageType)
        {
            if (_handlerMappings.ContainsKey(receiver))
            {
                var map = _handlerMappings[receiver];
                if (map.HandlerTypes.ContainsKey(messageType))
                {
                    return map.HandlerTypes[messageType];
                }
            }

            // Not found.
            return new List<Type>();
        }
        
        /// <summary>
        /// Map a message type to one or more handlers.
        /// </summary>
        protected void AddMessageType(IMessageReceiver receiver, Type messageType, Type[] handlerTypes)
        {
            lock (_mappingsLock)
            {
                try
                {
                    // Add the handler map.
                    if (!_handlerMappings.ContainsKey(receiver))
                    {
                        _handlerMappings.Add(receiver, new MessageHandlerMap());
                    }

                    // Get the current handler map.
                    var map = _handlerMappings[receiver];

                    // Check for duplicates.
                    if (!AllowMultipleMessageHandlers && map.HandlerTypes.ContainsKey(messageType))
                    {
                        throw new ProcessorConfigurationException("The message type {0} has already been registered with a handler.", messageType);
                    }

                    // Add each handler type ot the mappings.
                    foreach (var handlerType in handlerTypes)
                    {
                        map.Add(messageType, handlerType);

                        // Log.
                        MessengerEventSource.Log.RegisteredHandler(receiver, messageType, handlerType);
                    }
                }
                catch (Exception ex)
                {
                    // Log.
                    MessengerEventSource.Log.RegisteredHandlerError(receiver, messageType, ex);

                    // Rethrow.
                    throw;
                }
            }
        }

        /// <summary>
        /// Set a custom error handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public TConfiguration WithErrorHandler(OnMessageException handler)
        {
            ErrorHandler = handler;

            // Continue.
            return this as TConfiguration;
        }
    }
}