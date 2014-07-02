using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Send;
using RedDog.ServiceBus.Tests.Integration.TestUtils;

using Xunit;

namespace RedDog.ServiceBus.Tests.Integration.Send
{
    public class PartitionedMessageSenderFacts
    {
        [Fact]
        public async Task SendMessage()
        {
            using (var queue1 = ServiceBusEntityFactory.DeleteAndCreateQueue("integration-queue-sender1"))
            using (var queue2 = ServiceBusEntityFactory.DeleteAndCreateQueue("integration-queue-sender2"))
            using (var queue3 = ServiceBusEntityFactory.DeleteAndCreateQueue("integration-queue-sender3"))
            using (var queue4 = ServiceBusEntityFactory.DeleteAndCreateQueue("integration-queue-sender4"))
            {

                var sender = new PartitionedMessageSender("integration-partitioned-sender", new[]
                {
                    new QueueMessageSender(queue1.Client), 
                    new QueueMessageSender(queue2.Client), 
                    new QueueMessageSender(queue3.Client), 
                    new QueueMessageSender(queue4.Client)
                });

                var id1 = Guid.NewGuid().ToString();
                await sender.SendAsync(new BrokeredMessage { MessageId = id1 });

                var id2 = Guid.NewGuid().ToString();
                await sender.SendAsync(new BrokeredMessage { MessageId = id2 });

                var id3 = Guid.NewGuid().ToString();
                await sender.SendAsync(new BrokeredMessage { MessageId = id3 });

                var id4 = Guid.NewGuid().ToString();
                await sender.SendAsync(new BrokeredMessage { MessageId = id4 });

                var id5 = Guid.NewGuid().ToString();
                await sender.SendAsync(new BrokeredMessage { MessageId = id5 });

                var id6 = Guid.NewGuid().ToString();
                await sender.SendAsync(new BrokeredMessage { MessageId = id6 });


                var messages = queue1.Client.ReceiveBatch(250).ToArray();
                Assert.Equal(2, messages.Length);
                Assert.True(messages.Any(m => m.MessageId == id1));
                Assert.True(messages.Any(m => m.MessageId == id5));

                messages = queue2.Client.ReceiveBatch(250).ToArray();
                Assert.Equal(2, messages.Length);
                Assert.True(messages.Any(m => m.MessageId == id2));
                Assert.True(messages.Any(m => m.MessageId == id6));

                messages = queue3.Client.ReceiveBatch(250).ToArray();
                Assert.Equal(1, messages.Length);
                Assert.True(messages.Any(m => m.MessageId == id3));

                messages = queue4.Client.ReceiveBatch(250).ToArray();
                Assert.Equal(1, messages.Length);
                Assert.True(messages.Any(m => m.MessageId == id4));
            }
        }
    }
}
