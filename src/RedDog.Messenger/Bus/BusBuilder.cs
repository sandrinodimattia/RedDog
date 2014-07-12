using System;
using System.Collections.Generic;

using RedDog.Messenger.Configuration;
using RedDog.Messenger.Diagnostics;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public abstract class BusBuilder<TConfiguration> : MessagingConfiguration<TConfiguration>, IBusConfiguration<TConfiguration>
        where TConfiguration : class, IBusConfiguration<TConfiguration>
    {
        private readonly Dictionary<Type, IMessageSender> _mappings;

        private readonly object _mappingsLock = new object();

        /// <summary>
        /// List of message types mapped to a sender.
        /// </summary>
        public IReadOnlyDictionary<Type, IMessageSender> MessageTypes
        {
            get { return _mappings; }
        }

        protected BusBuilder()
        {
            _mappings = new Dictionary<Type, IMessageSender>();
        }

        /// <summary>
        /// Find the sender matching a specific message type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IMessageSender GetSender(Type type)
        {
            if (!MessageTypes.ContainsKey(type))
            {
                throw new BusConfigurationException("The type {0} has not been registered with a sender.");
            }

            return MessageTypes[type];
        }

        /// <summary>
        /// Map a message type to a sender.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sender"></param>
        protected void AddMessageType(Type type, IMessageSender sender)
        {
            lock (_mappingsLock)
            {
                if (_mappings.ContainsKey(type))
                {
                    throw new BusConfigurationException("The type {0} has already been registered with a sender.");
                }

                // Log.
                MessengerEventSource.Log.RegisteredMessageSender(sender, type);

                // Add mapping.
                _mappings.Add(type, sender);
            }
        }
    }
}