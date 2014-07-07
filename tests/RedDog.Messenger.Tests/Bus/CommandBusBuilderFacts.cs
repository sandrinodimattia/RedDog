using FakeItEasy;
using RedDog.Messenger.Bus;
using RedDog.Messenger.Composition;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.ServiceBus.Send;
using Xunit;

namespace RedDog.Messenger.Tests.Bus
{
    public class CommandBusBuilderFacts
    {
        [Fact]
        public void WithContainerShouldStoreContainer()
        {
            // Arrange.
            var configuration = CommandBusBuilder.Create();
            var container = A.Fake<IContainer>();
            
            // Act.
            configuration.WithContainer(container);
            
            // Assert.
            Assert.Equal(configuration.Container, container);
        }

        [Fact]
        public void RegisterCommandsShouldRegisterSingleType()
        {
            // Arrange.
            var submitSender = A.Fake<IMessageSender>();
            var deleteSender = A.Fake<IMessageSender>();

            // Act.
            var configuration = CommandBusBuilder.Create()
                .RegisterCommand<SubmitOrderCommand>(submitSender)
                .RegisterCommand<DeleteOrderCommand>(deleteSender);

            // Assert.
            Assert.Equal(2, configuration.MessageTypes.Count);
            Assert.Equal(submitSender, configuration.MessageTypes[typeof(SubmitOrderCommand)]);
            Assert.Equal(deleteSender, configuration.MessageTypes[typeof(DeleteOrderCommand)]);
        }

        [Fact]
        public void RegisterCommandsShouldRegisterTypeArray()
        {
            // Arrange.
            var sender = A.Fake<IMessageSender>();

            // Act.
            var configuration = CommandBusBuilder.Create()
                .RegisterCommands(sender, typeof(SubmitOrderCommand), typeof(DeleteOrderCommand));

            // Assert.
            Assert.Equal(2, configuration.MessageTypes.Count);
            Assert.Equal(sender, configuration.MessageTypes[typeof(SubmitOrderCommand)]);
            Assert.Equal(sender, configuration.MessageTypes[typeof(DeleteOrderCommand)]);
        }

        [Fact]
        public void RegisterCommandsShouldSupportFluentRegistration()
        {
            // Arrange.
            var submitSender = A.Fake<IMessageSender>();
            var deleteSender = A.Fake<IMessageSender>();

            // Act.
            var configuration = CommandBusBuilder.Create()
                .RegisterCommands(submitSender, 
                    c => c.With<CreateOrderCommand>()
                          .With<SubmitOrderCommand>()
                          .With<ConfirmOrderCommand>())
                .RegisterCommands(deleteSender,
                    c => c.With<DeleteOrderCommand>());

            // Assert.
            Assert.Equal(4, configuration.MessageTypes.Count);
            Assert.Equal(submitSender, configuration.MessageTypes[typeof(CreateOrderCommand)]);
            Assert.Equal(submitSender, configuration.MessageTypes[typeof(SubmitOrderCommand)]);
            Assert.Equal(submitSender, configuration.MessageTypes[typeof(ConfirmOrderCommand)]);
            Assert.Equal(deleteSender, configuration.MessageTypes[typeof(DeleteOrderCommand)]);
        }

        [Fact]
        public void RegisterCommandsShouldSupportAssembly()
        {
            // Arrange.
            var sender = A.Fake<IMessageSender>();

            // Act.
            var configuration = CommandBusBuilder.Create()
                .RegisterCommands(sender, GetType().Assembly);

            // Assert.
            Assert.Equal(5, configuration.MessageTypes.Count);
            Assert.Equal(sender, configuration.MessageTypes[typeof(CreateOrderCommand)]);
            Assert.Equal(sender, configuration.MessageTypes[typeof(SubmitOrderCommand)]);
            Assert.Equal(sender, configuration.MessageTypes[typeof(ConfirmOrderCommand)]);
            Assert.Equal(sender, configuration.MessageTypes[typeof(DeleteOrderCommand)]);
        }

        [Fact]
        public void GetSenderShouldReturnRightSender()
        {
            // Arrange.
            var submitSender = A.Fake<IMessageSender>();
            var deleteSender = A.Fake<IMessageSender>();

            // Act.
            var configuration = CommandBusBuilder.Create()
                .RegisterCommands(submitSender,
                    c => c.With<CreateOrderCommand>()
                          .With<SubmitOrderCommand>()
                          .With<ConfirmOrderCommand>())
                .RegisterCommands(deleteSender,
                    c => c.With<DeleteOrderCommand>());

            // Assert.
            Assert.Equal(submitSender, configuration.GetSender(typeof(CreateOrderCommand)));
            Assert.Equal(submitSender, configuration.GetSender(typeof(SubmitOrderCommand)));
            Assert.Equal(submitSender, configuration.GetSender(typeof(ConfirmOrderCommand)));
            Assert.Equal(deleteSender, configuration.GetSender(typeof(DeleteOrderCommand)));
        }
    }
}
