using System;

namespace RedDog.ServiceBus.Receive
{
    public class MessageReceiverException : Exception
    {
        public MessageReceiverException(string message)
            : base(message)
        {


        }
    }
}