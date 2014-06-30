using System;

namespace RedDog.ServiceBus.Receive.Session
{
    public class OnSessionMessageOptions
    {
        public bool RequireSequentialProcessing
        {
            get;
            set;
        }

        public TimeSpan AutoRenewSessionTimeout
        {
            get;
            set;
        }

        public TimeSpan MessageWaitTimeout
        {
            get;
            set;
        }

        public int MaxConcurrentSessions
        {
            get;
            set;
        }

        public bool AutoComplete
        {
            get;
            set;
        }

        public OnSessionMessageOptions()
        {
            AutoComplete = true;
            MessageWaitTimeout = TimeSpan.FromMinutes(1.0);
            AutoRenewSessionTimeout = TimeSpan.FromMinutes(5.0);
            MaxConcurrentSessions = Environment.ProcessorCount * 1000;
            RequireSequentialProcessing = false;
        }
    }
}