using FakeItEasy;
using RedDog.Messenger.Processor;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.Messenger.Tests.Processor.Handlers;
using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;
using Xunit;

namespace RedDog.Messenger.Tests.Processor
{
    public class CommandProcessorBuilderFacts
    {
        [Fact]
        public void CreateShouldReturnBuilder()
        {
            // Act.
            var config = CommandProcessorBuilder.Create();

            // Assert.
            Assert.NotNull(config);
        }

        [Fact]
        public void BuildShouldCreateProcessor()
        {
            // Act.
            var processor = CommandProcessorBuilder.Create()
                .Build();

            // Assert.
            Assert.NotNull(processor);
        }

        [Fact]
        public void RegisterCommandHandlerShouldNotAllowMutlipleRegistrationsForTheSameCommand()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            Assert.Throws<ProcessorConfigurationException>(() =>
            {
                config.RegisterCommandHandler<RemoveOrderCommandHandler>(receiver);
                config.RegisterCommandHandler<RemoveOrderCommandHandler>(receiver);
            });
        }

        [Fact]
        public void RegisterCommandHandlerShouldRegisterHandler()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config.RegisterCommandHandler<RemoveOrderCommandHandler>(receiver);

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(2, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(DeleteOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(CancelOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
        }

        [Fact]
        public void RegisterCommandHandlerShouldRegisterHandlerForSessions()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<ISessionMessagePump>();

            // Act.
            config.RegisterCommandHandler<RemoveOrderCommandHandler>(receiver);

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(2, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(DeleteOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(CancelOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
        }

        [Fact]
        public void RegisterCommandHandlersShouldRegisterHandlerArray()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config.RegisterCommandHandlers(receiver, new []
            {
                typeof(RemoveOrderCommandHandler),
                typeof(ConfirmOrderCommandHandler)
            });

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(DeleteOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(CancelOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(ConfirmOrderCommand)][0] == typeof(ConfirmOrderCommandHandler));
        }

        [Fact]
        public void RegisterCommandHandlersShouldRegisterHandlerArrayForSessions()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<ISessionMessagePump>();

            // Act.
            config.RegisterCommandHandlers(receiver, new[]
            {
                typeof(RemoveOrderCommandHandler),
                typeof(ConfirmOrderCommandHandler)
            });

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(DeleteOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(CancelOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(ConfirmOrderCommand)][0] == typeof(ConfirmOrderCommandHandler));
        }
        
        [Fact]
        public void RegisterCommandHandlersShouldRegisterFluent()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config = config.RegisterCommandHandlers(receiver, cfg => 
                cfg.With<RemoveOrderCommandHandler>()
                   .With<ConfirmOrderCommandHandler>());

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(DeleteOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(CancelOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(ConfirmOrderCommand)][0] == typeof(ConfirmOrderCommandHandler));
        }

        [Fact]
        public void RegisterCommandHandlersShouldRegisterFluentForSessions()
        {
            // Arrange.
            var config = CommandProcessorBuilder.Create();
            var receiver = A.Fake<ISessionMessagePump>();

            // Act.
            config = config.RegisterCommandHandlers(receiver, cfg =>
                cfg.With<RemoveOrderCommandHandler>()
                   .With<ConfirmOrderCommandHandler>());

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(DeleteOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(CancelOrderCommand)][0] == typeof(RemoveOrderCommandHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(ConfirmOrderCommand)][0] == typeof(ConfirmOrderCommandHandler));
        }

        [Fact]
        public void GetReceiversShouldReturnCorrectHandlers()
        {
            // Arrange.
            var receiver = A.Fake<IMessagePump>();
            var config = CommandProcessorBuilder.Create().RegisterCommandHandlers(receiver, new[]
            {
                typeof(RemoveOrderCommandHandler),
                typeof(ConfirmOrderCommandHandler)
            });

            // Act.
            var Receivers = config.GetHandlerTypes(receiver, typeof(CancelOrderCommand));

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(1, Receivers.Count);
            Assert.Equal(typeof(RemoveOrderCommandHandler), Receivers[0]);
        }

        [Fact]
        public void GetReceiversShouldReturnEmptyIfNothingFound()
        {
            // Arrange.
            var receiver = A.Fake<IMessagePump>();
            var config = CommandProcessorBuilder.Create().RegisterCommandHandlers(receiver, new[]
            {
                typeof(ConfirmOrderCommandHandler)
            });

            // Act.
            var Receivers = config.GetHandlerTypes(receiver, typeof(CancelOrderCommand));

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(0, Receivers.Count);
        }
    }
}
