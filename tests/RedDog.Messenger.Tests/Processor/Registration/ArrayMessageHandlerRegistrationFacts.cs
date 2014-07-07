using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.Messenger.Tests.Processor.Handlers;
using Xunit;

namespace RedDog.Messenger.Tests.Processor.Registration
{
    public class ArrayMessageHandlerRegistrationFacts
    {
        [Fact]
        public void GetRegistrationsShouldSupportMultipleRecord()
        {
            // Arrange.
            var handler = new ArrayMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>),
                typeof(ConfirmOrderCommandHandler), typeof(RemoveOrderCommandHandler));

            // Act.
            var registrations = handler.GetRegistrations();

            // Assert.
            Assert.Equal(3, registrations.Count);
            Assert.Equal(typeof(ConfirmOrderCommandHandler), registrations[typeof(ConfirmOrderCommand)][0]);
            Assert.Equal(typeof(RemoveOrderCommandHandler), registrations[typeof(DeleteOrderCommand)][0]);
            Assert.Equal(typeof(RemoveOrderCommandHandler), registrations[typeof(CancelOrderCommand)][0]);
        }
    }
}
