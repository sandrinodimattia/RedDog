using System;
using System.Collections.Generic;
using System.Linq;

using RedDog.Messenger.Contracts.Handlers;

namespace RedDog.Messenger.Processor.Registration
{
    internal abstract class MessageHandlerRegistration<TMessageHandler> : IMessageHandlerRegistration<TMessageHandler>
        where TMessageHandler : IMessageHandler
    {
        private readonly Type _handlerInterface;

        private readonly object _typesLock = new object();

        private readonly IDictionary<Type, IList<Type>> _handlerMappings;

        /// <summary>
        /// Initialize the handler registration.
        /// </summary>
        protected MessageHandlerRegistration(Type handlerInterface)
        {
            _handlerInterface = handlerInterface;
            _handlerMappings = new Dictionary<Type, IList<Type>>();
        }

        /// <summary>
        /// Get the available registrations.
        /// </summary>
        /// <returns></returns>
        public IDictionary<Type, IList<Type>> GetRegistrations()
        {
            return _handlerMappings;
        }

        /// <summary>
        /// Register the message supported by a handler.
        /// </summary>
        /// <param name="handlerType"></param>
        protected void RegisterMessageTypes(Type handlerType)
        {
            lock (_typesLock)
            {
                // Get all the message types supported by the handler.
                var messageTypes = handlerType
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == _handlerInterface)
                    .Select(i => i.GetGenericArguments()[0])
                    .ToList();
                
                // Register the handler for each message type.
                foreach (var messageType in messageTypes)
                {
                    if (!_handlerMappings.ContainsKey(messageType))
                    {
                        _handlerMappings.Add(messageType, new List<Type>());
                    }

                    // Register the handler.
                    _handlerMappings[messageType].Add(handlerType);
                }
            }
        }
    }
}