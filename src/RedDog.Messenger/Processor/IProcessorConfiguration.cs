using System;
using System.Collections.Generic;

using RedDog.Messenger.Configuration;

using RedDog.ServiceBus.Receive;

namespace RedDog.Messenger.Processor
{
    public interface IProcessorConfiguration<out TConfiguration> : IProcessorConfiguration, IMessagingConfiguration, IMessagingReceiverConfiguration<TConfiguration>, IMessagingContainerConfiguration<TConfiguration>, IMessagingSerializerConfiguration<TConfiguration>
        where TConfiguration : IMessagingConfiguration
    {

    }

    public interface IProcessorConfiguration
    {

        /// <summary>
        /// Error handler for custom logging etc..
        /// </summary>
        OnMessageException ErrorHandler
        {
            get;
        }

        /// <summary>
        /// List of message types mapped to one or more handlers.
        /// </summary>
        IReadOnlyDictionary<object, MessageHandlerMap> Receivers
        {
            get;
        }

        /// <summary>
        /// Find the handlers matching a specific message type.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        IReadOnlyList<Type> GetHandlerTypes(object receiver, Type messageType);
    }
}