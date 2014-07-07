using System;

namespace RedDog.Messenger.Processor
{
    public class ProcessorException : Exception
    {
        public ProcessorException(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}