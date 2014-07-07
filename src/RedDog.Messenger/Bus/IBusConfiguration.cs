using System;
using System.Collections.Generic;

using RedDog.Messenger.Configuration;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public interface IBusConfiguration<out TConfiguration> : IMessagingConfiguration, IMessagingContainerConfiguration<TConfiguration>, IMessagingSerializerConfiguration<TConfiguration>
        where TConfiguration : IMessagingConfiguration
    {
        /// <summary>
        /// List of message types mapped to a sender.
        /// </summary>
        IReadOnlyDictionary<Type, IMessageSender> MessageTypes
        {
            get;
        }

        /// <summary>
        /// Find the sender matching a specific message type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IMessageSender GetSender(Type type);
    }
}