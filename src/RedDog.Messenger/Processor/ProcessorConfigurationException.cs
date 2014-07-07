using System;

namespace RedDog.Messenger.Processor
{
    public class ProcessorConfigurationException : Exception
    {
        public ProcessorConfigurationException(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}