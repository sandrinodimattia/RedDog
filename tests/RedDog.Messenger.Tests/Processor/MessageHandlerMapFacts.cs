using RedDog.Messenger.Processor;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.Messenger.Tests.Processor.Handlers;
using Xunit;

namespace RedDog.Messenger.Tests.Processor
{

    public class MessageHandlerMapFacts
    {
        [Fact]
        public void AddShouldAllowMultipleCommandRegistration()
        {
            // Arrange.
            var map = new MessageHandlerMap();

            // Act.
            map.Add(typeof(CancelOrderCommand), typeof(RemoveOrderCommandHandler));
            map.Add(typeof(DeleteOrderCommand), typeof(RemoveOrderCommandHandler));
            map.Add(typeof(DeleteOrderCommand), typeof(CreateOrderCommand));

            // Assert.
            var registrations = map.GetHandlerTypes();
            Assert.Equal(2, registrations.Count);
            Assert.Equal(typeof(CreateOrderCommand), registrations[typeof(DeleteOrderCommand)][1]);
        }
    }
}
