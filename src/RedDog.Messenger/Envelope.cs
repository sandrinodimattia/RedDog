using System;
using System.Collections.Generic;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger
{
    public class Envelope<TMessage> : Envelope, IEnvelope<TMessage>
        where TMessage : IMessage
    {
        internal Envelope(TMessage body)
        {
            Body = body;
            Properties = new Dictionary<string, object>();
        }

        public TimeSpan Delay
        {
            get;
            set;
        }

        public TimeSpan TimeToLive
        {
            get;
            set;
        }

        public string MessageId
        {
            get
            {
                if (EqualityComparer<TMessage>.Default.Equals(Body, default(TMessage)))
                    return null;
                return Body.Id;
            }
        }

        public string CorrelationId
        {
            get;
            set;
        }

        public string SessionId
        {
            get;
            set;
        }

        public IDictionary<string, object> Properties
        {
            get;
            set;
        }

        public TMessage Body
        {
            get;
            set;
        }
    }
}