using System;
using System.Collections.Generic;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger
{
    public abstract class Envelope
    {
        public static Envelope<TMessage> Create<TMessage>(TMessage message, string correlationId = null, string sessionId = null)
            where TMessage : class, IMessage
        {
            // Force the message identifier.
            if (message != null && String.IsNullOrEmpty(message.Id))
            {
                message.Id = Guid.NewGuid().ToString("N").ToLower();
            }

            // Create envelope.
            return new Envelope<TMessage>(message)
            {
                CorrelationId = correlationId ?? (message != null ? message.Id : String.Empty),
                SessionId = sessionId,
                Properties = new Dictionary<string, object>()
            };
        }
    }
}