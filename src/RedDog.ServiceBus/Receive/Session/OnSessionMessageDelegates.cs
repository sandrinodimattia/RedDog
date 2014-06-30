using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive.Session
{
    public delegate Task OnSessionMessage(MessageSession session, BrokeredMessage message);

    public delegate Task OnSessionMessageException(string action, Exception exception);
}
