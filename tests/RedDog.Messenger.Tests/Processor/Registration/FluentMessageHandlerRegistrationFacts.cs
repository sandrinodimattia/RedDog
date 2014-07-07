using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.Messenger.Tests.Processor.Handlers;
using Xunit;

namespace RedDog.Messenger.Tests.Processor.Registration
{
    public class FluentMessageHandlerRegistrationFacts
    {
        [Fact]
        public void WithShouldRegisterHandlersCorrectly()
        {
            // Arrange.
            var handler = new FluentMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>));
            
            // Act.
            handler
                .With<ConfirmOrderCommandHandler>()
                .With<RemoveOrderCommandHandler>();
            var registrations = handler.GetRegistrations();

            // Assert.
            Assert.Equal(3, registrations.Count);
            Assert.Equal(typeof(ConfirmOrderCommandHandler), registrations[typeof(ConfirmOrderCommand)][0]);
            Assert.Equal(typeof(RemoveOrderCommandHandler), registrations[typeof(DeleteOrderCommand)][0]);
            Assert.Equal(typeof(RemoveOrderCommandHandler), registrations[typeof(CancelOrderCommand)][0]);
        }
    }
}
