using FakeItEasy;
using RedDog.Messenger.Bus;
using RedDog.Messenger.Configuration;
using RedDog.Messenger.Filters;
using RedDog.Messenger.Serialization;
using RedDog.Messenger.Tests.Bus.Commands;
using Xunit;

namespace RedDog.Messenger.Tests.Configuration
{
    public class MessagingConfigurationFacts
    {
        [Fact]
        public void WithSerializerShouldRegisterSerializer()
        {
            // Arrange.
            var config = new MessagingConfiguration<ICommandBusConfiguration>();
            var serializer = A.Fake<ISerializer>();

            // Act.
            config.WithSerializer(serializer);

            // Assert.
            Assert.Equal(serializer, config.Serializer);
        }
        [Fact]
        public void WithSerializationFilterShouldRegisterFilter()
        {
            // Arrange.
            var config = new MessagingConfiguration<ICommandBusConfiguration>();
            var filter = A.Fake<IMessageFilter>();

            // Act.
            config.WithSerializationFilter(filter);
            config.MessageFilterInvoker.BeforeDeserialization(new Envelope<CreateOrderCommand>(new CreateOrderCommand()), new byte[0])
                .Wait();

            // Assert.
            A.CallTo(() => filter.BeforeDeserialization(A<Envelope<CreateOrderCommand>>.Ignored, A<byte[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
