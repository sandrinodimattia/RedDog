using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Tests.Bus.Commands;
using Xunit;

namespace RedDog.Messenger.Tests.Bus
{
    public class ArrayMessageRegistrationFacts
    {
        [Fact]
        public void GetTypesShouldReturnCorrectType()
        {
            // Arrange.
            var source = new ArrayMessageRegistration<ICommand>(
                typeof(SubmitOrderCommand),
                typeof(DeleteOrderCommand));
            
            // Act.
            var types = source.GetTypes();

            // Assert.
            Assert.Equal(2, types.Length);
            Assert.Equal(typeof(SubmitOrderCommand), types[0]);
            Assert.Equal(typeof(DeleteOrderCommand), types[1]);
        }
    }
}
