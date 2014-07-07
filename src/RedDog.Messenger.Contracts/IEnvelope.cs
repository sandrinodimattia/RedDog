using System;
using System.Collections.Generic;

namespace RedDog.Messenger.Contracts
{
    public interface IEnvelope
    {
        TimeSpan Delay
        {
            get;
        }

        TimeSpan TimeToLive
        {
            get;
        }

        string MessageId
        {
            get;
        }

        string SessionId
        {
            get;
        }

        string CorrelationId
        {
            get;
        }

        IDictionary<string, object> Properties
        {
            get;
        }
    }
}