using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.ServiceBus.Send;
using RedDog.ServiceBus.Tests.Integration.TestUtils;

using Xunit;

namespace RedDog.ServiceBus.Tests.Integration.Send
{
    public class TopicMessageSenderFacts
    {
        [Fact]
        public async Task SendMessage()
        {
            using (var topic = ServiceBusEntityFactory.DeleteAndCreateTopic("integration-topic-sender"))
            {
                using (var subscription = ServiceBusEntityFactory.DeleteAndCreateTopicSubscription("integration-topic-sender", "sub1"))
                {
                    var id = Guid.NewGuid().ToString();

                    var sender = new TopicMessageSender(topic.Client);
                    await sender.SendAsync(new BrokeredMessage {MessageId = id});

                    var messages = subscription.Client.ReceiveBatch(250).ToArray();
                    Assert.Equal(1, messages.Length);
                    Assert.Equal(id, messages[0].MessageId);
                }
            }
        }
        [Fact]
        public async Task SendBatchMessages()
        {
            using (var topic = ServiceBusEntityFactory.DeleteAndCreateTopic("integration-topic-sender"))
            {
                using (var subscription = ServiceBusEntityFactory.DeleteAndCreateTopicSubscription("integration-topic-sender", "sub1"))
                {
                    var id1 = Guid.NewGuid().ToString();
                    var id2 = Guid.NewGuid().ToString();
                    var id3 = Guid.NewGuid().ToString();

                    var sender = new TopicMessageSender(topic.Client);
                    await sender.SendBatchAsync(new[]
                    {
                        new BrokeredMessage {MessageId = id1},
                        new BrokeredMessage {MessageId = id2},
                        new BrokeredMessage {MessageId = id3}
                    });

                    var messages = subscription.Client.ReceiveBatch(250).ToArray();
                    Assert.Equal(3, messages.Length);
                    Assert.True(messages.Any(m => m.MessageId == id1));
                    Assert.True(messages.Any(m => m.MessageId == id2));
                    Assert.True(messages.Any(m => m.MessageId == id3));
                }
            }
        }
    }
}