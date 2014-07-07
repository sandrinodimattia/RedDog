using System;
using System.Collections.Generic;

using RedDog.Messenger.Contracts.Handlers;

namespace RedDog.Messenger.Processor.Registration
{
    public interface IMessageHandlerRegistration<TMessageHandler>
        where TMessageHandler : IMessageHandler
    {
        IDictionary<Type, IList<Type>> GetRegistrations();
    }
}