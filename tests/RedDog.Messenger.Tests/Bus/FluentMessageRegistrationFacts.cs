using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Tests.Bus.Commands;
using Xunit;

namespace RedDog.Messenger.Tests.Bus
{
    public class FluentMessageHandlerRegistrationFacts
    {
        [Fact]
        public void GetTypesShouldReturnCorrectTypes()
        {
            // Arrange.
            var source = new FluentMessageRegistration<ICommand>()
                .With<SubmitOrderCommand>()
                .With<DeleteOrderCommand>();
            
            // Act.
            var types = source.GetTypes();

            // Assert.
            Assert.Equal(2, types.Length);
            Assert.Equal(typeof(DeleteOrderCommand), types[0]);
            Assert.Equal(typeof(SubmitOrderCommand), types[1]);
        }
    }
}
