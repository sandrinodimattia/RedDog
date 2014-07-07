using System;

namespace RedDog.Messenger.Bus
{
    public class BusConfigurationException : Exception
    {
        public BusConfigurationException(string message, params object[] args)
            : base(String.Format(message, args))
        {
        }
    }
}