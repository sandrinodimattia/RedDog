using FakeItEasy;
using RedDog.Messenger.Processor;
using RedDog.Messenger.Tests.Bus.Events;
using RedDog.Messenger.Tests.Processor.Handlers;
using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;
using Xunit;

namespace RedDog.Messenger.Tests.Processor
{
    public class EventProcessorBuilderFacts
    {
        [Fact]
        public void CreateShouldReturnBuilder()
        {
            // Act.
            var config = EventProcessorBuilder.Create();

            // Assert.
            Assert.NotNull(config);
        }

        [Fact]
        public void BuildShouldCreateProcessor()
        {
            // Act.
            var processor = EventProcessorBuilder.Create()
                .Build();

            // Assert.
            Assert.NotNull(processor);
        }

        [Fact]
        public void RegisterEventHandlerShouldAllowMultipleHandlersForTheSameMessage()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config.RegisterEventHandler<OrderRemovedEventHandler>(receiver);
            config.RegisterEventHandler<OrderRemovedEventHandler>(receiver);

            // Assert.
            Assert.Equal(2, config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)].Count);
            Assert.Equal(2, config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)].Count);
        }

        [Fact]
        public void RegisterEventHandlerShouldRegisterHandler()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config.RegisterEventHandler<OrderRemovedEventHandler>(receiver);

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(2, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)][0] == typeof(OrderRemovedEventHandler));
        }

        [Fact]
        public void RegisterEventHandlerShouldRegisterHandlerForSessions()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<ISessionMessagePump>();

            // Act.
            config.RegisterEventHandler<OrderRemovedEventHandler>(receiver);

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(2, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)][0] == typeof(OrderRemovedEventHandler));
        }

        [Fact]
        public void RegisterEventHandlersShouldRegisterHandlerArray()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config.RegisterEventHandlers(receiver, new []
            {
                typeof(OrderRemovedEventHandler),
                typeof(OrderConfirmedEventHandler)
            });

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderConfirmedEvent)][0] == typeof(OrderConfirmedEventHandler));
        }

        [Fact]
        public void RegisterEventHandlersShouldRegisterHandlerArrayForSessions()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<ISessionMessagePump>();

            // Act.
            config.RegisterEventHandlers(receiver, new[]
            {
                typeof(OrderRemovedEventHandler),
                typeof(OrderConfirmedEventHandler)
            });

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderConfirmedEvent)][0] == typeof(OrderConfirmedEventHandler));
        }
        
        [Fact]
        public void RegisterEventHandlersShouldRegisterFluent()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<IMessagePump>();

            // Act.
            config = config.RegisterEventHandlers(receiver, cfg => 
                cfg.With<OrderRemovedEventHandler>()
                   .With<OrderConfirmedEventHandler>());

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderConfirmedEvent)][0] == typeof(OrderConfirmedEventHandler));
        }

        [Fact]
        public void RegisterEventHandlersShouldRegisterFluentForSessions()
        {
            // Arrange.
            var config = EventProcessorBuilder.Create();
            var receiver = A.Fake<ISessionMessagePump>();

            // Act.
            config = config.RegisterEventHandlers(receiver, cfg =>
                cfg.With<OrderRemovedEventHandler>()
                   .With<OrderConfirmedEventHandler>());

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(3, config.Receivers[receiver].HandlerTypes.Count);
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderDeletedEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderCancelledEvent)][0] == typeof(OrderRemovedEventHandler));
            Assert.True(config.Receivers[receiver].HandlerTypes[typeof(OrderConfirmedEvent)][0] == typeof(OrderConfirmedEventHandler));
        }

        [Fact]
        public void GetReceiversShouldReturnCorrectHandlers()
        {
            // Arrange.
            var receiver = A.Fake<IMessagePump>();
            var config = EventProcessorBuilder.Create().RegisterEventHandlers(receiver, new[]
            {
                typeof(OrderRemovedEventHandler),
                typeof(OrderConfirmedEventHandler)
            });

            // Act.
            var Receivers = config.GetHandlerTypes(receiver, typeof(OrderCancelledEvent));

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(1, Receivers.Count);
            Assert.Equal(typeof(OrderRemovedEventHandler), Receivers[0]);
        }

        [Fact]
        public void GetReceiversShouldReturnEmptyIfNothingFound()
        {
            // Arrange.
            var receiver = A.Fake<IMessagePump>();
            var config = EventProcessorBuilder.Create().RegisterEventHandlers(receiver, new[]
            {
                typeof(OrderConfirmedEventHandler)
            });

            // Act.
            var Receivers = config.GetHandlerTypes(receiver, typeof(OrderCancelledEvent));

            // Assert.
            Assert.True(config.Receivers.ContainsKey(receiver));
            Assert.Equal(0, Receivers.Count);
        }
    }
}
