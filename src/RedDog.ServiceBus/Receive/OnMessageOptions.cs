using System;

namespace RedDog.ServiceBus.Receive
{
    public class OnMessageOptions
    {
        public TimeSpan AutoRenewTimeout
        {
            get;
            set;
        }

        public int MaxConcurrentCalls
        {
            get;
            set;
        }

        public bool AutoComplete
        {
            get;
            set;
        }

        public OnMessageOptions()
        {
            AutoComplete = true;
            MaxConcurrentCalls = 1;
            AutoRenewTimeout = TimeSpan.FromMinutes(5.0);
        }
    }
}