using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Send
{
    public interface IMessageSender
    {
        string Path
        {
            get;
        }

        Task SendAsync(BrokeredMessage message);

        Task SendBatchAsync(BrokeredMessage[] messages);
    }
}
