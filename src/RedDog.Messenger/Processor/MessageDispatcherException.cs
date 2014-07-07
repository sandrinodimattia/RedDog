using System;

namespace RedDog.Messenger.Processor
{
    public class MessageDispatcherException : Exception
    {
        public MessageDispatcherException(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}