using System.Linq;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor.Registration;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.Messenger.Tests.Processor.Handlers;
using Xunit;

namespace RedDog.Messenger.Tests.Processor.Registration
{
    public class TypeMessageHandlerRegistrationFacts
    {
        [Fact]
        public void GetRegistrationsShouldReturnSingleRecord()
        {
            // Arrange.
            var handler = new TypeMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), typeof(ConfirmOrderCommandHandler));
            
            // Act.
            var registrations = handler.GetRegistrations();

            // Assert.
            Assert.Equal(1, registrations.Count);
            Assert.Equal(typeof(ConfirmOrderCommand), registrations.FirstOrDefault().Key);
            Assert.Equal(typeof(ConfirmOrderCommandHandler), registrations.FirstOrDefault().Value[0]);
        }

        [Fact]
        public void GetRegistrationsShouldSupportMultipleRecord()
        {
            // Arrange.
            var handler = new TypeMessageHandlerRegistration<ICommandHandler>(typeof(ICommandHandler<>), typeof(RemoveOrderCommandHandler));

            // Act.
            var registrations = handler.GetRegistrations();

            // Assert.
            Assert.Equal(2, registrations.Count);
            Assert.Equal(typeof(RemoveOrderCommandHandler), registrations[typeof(DeleteOrderCommand)][0]);
            Assert.Equal(typeof(RemoveOrderCommandHandler), registrations[typeof(CancelOrderCommand)][0]);
        }
    }
}
