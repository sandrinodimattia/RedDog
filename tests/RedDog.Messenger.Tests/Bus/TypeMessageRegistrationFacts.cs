using System.Linq;
using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Tests.Bus.Commands;
using Xunit;

namespace RedDog.Messenger.Tests.Bus
{
    public class TypeMessageRegistrationFacts
    {
        [Fact]
        public void GetTypesShouldReturnCorrectType()
        {
            // Arrange.
            var source = new TypeMessageRegistration<ICommand>(typeof(CreateOrderCommand));
            
            // Act.
            var types = source.GetTypes();

            // Assert.
            Assert.Equal(1, types.Length);
            Assert.Equal(typeof(CreateOrderCommand), types.FirstOrDefault());
        }
    }
}
