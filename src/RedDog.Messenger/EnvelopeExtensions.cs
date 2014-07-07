using System;
using System.Collections.Generic;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger
{
    public static class EnvelopeExtensions
    {
        public static Envelope<TMessage> Body<TMessage>(this Envelope<TMessage> envelope, TMessage body)
            where TMessage : IMessage
        {
            envelope.Body = body;
            return envelope;
        }

        public static Envelope<TMessage> Delayed<TMessage>(this Envelope<TMessage> envelope, TimeSpan delay)
            where TMessage : IMessage
        {
            envelope.Delay = delay;
            return envelope;
        }

        public static Envelope<TMessage> TimeToLive<TMessage>(this Envelope<TMessage> envelope, TimeSpan timeToLive)
            where TMessage : IMessage
        {
            envelope.TimeToLive = timeToLive;
            return envelope;
        }
        
        public static Envelope<TMessage> CorrelationId<TMessage>(this Envelope<TMessage> envelope, string correlationId)
            where TMessage : IMessage
        {
            envelope.CorrelationId = correlationId;
            return envelope;
        }

        public static Envelope<TMessage> SessionId<TMessage>(this Envelope<TMessage> envelope, string sessionId)
            where TMessage : IMessage
        {
            envelope.SessionId = sessionId;
            return envelope;
        }

        public static Envelope<TMessage> Property<TMessage>(this Envelope<TMessage> envelope, string key, object value)
            where TMessage : IMessage
        {
            envelope.Properties.Add(key, value);
            return envelope;
        }

        public static Envelope<TMessage> Properties<TMessage>(this Envelope<TMessage> envelope, IDictionary<string, object> properties)
            where TMessage : IMessage
        {
            foreach (var item in properties)
                envelope.Properties.Add(item);
            return envelope;
        }
    }
}