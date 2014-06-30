using System;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive
{
    public delegate Task OnMessage(BrokeredMessage message);

    public delegate Task OnMessageException(string action, Exception exception);
}
