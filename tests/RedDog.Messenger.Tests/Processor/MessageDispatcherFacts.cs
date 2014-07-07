using System;
using System.Threading.Tasks;
using FakeItEasy;
using RedDog.Messenger.Composition;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Processor;
using RedDog.Messenger.Tests.Bus.Commands;
using RedDog.Messenger.Tests.Bus.Events;
using RedDog.Messenger.Tests.Processor.Handlers;
using Xunit;

namespace RedDog.Messenger.Tests.Processor
{
    public class MessageDispatcherFacts
    {
        [Fact]
        public void DispatchShouldThrowExceptionIfHandlerNotFound()
        {
            // Arrange.
            var map = new MessageHandlerMap();
            map.Add(typeof(CancelOrderCommand), typeof(RemoveOrderCommandHandler));
            var container = A.Fake<IContainer>();
            var dispatcher = new MessageDispatcher(container, map);

            // Act.
            Assert.Throws<MessageDispatcherException>(() =>
            {
                try
                {
                    dispatcher.Dispatch("CreateOrder", new Envelope<CreateOrderCommand>(new CreateOrderCommand())).Wait();
                }
                catch (AggregateException ex)
                {
                    throw ex.InnerException;
                }
            });
        }

        [Fact]
        public void DispatchShouldExecuteHandler()
        {
            // Arrange.
            var command = new CancelOrderCommand();

            var executed = false;
            var handler = A.Fake<ICommandHandler<CancelOrderCommand>>();
            A.CallTo(() => handler.Handle(command)).Invokes(c => { executed = true; }).Returns(Task.FromResult(0));
            
            var map = new MessageHandlerMap();
            map.Add(typeof(CancelOrderCommand), handler.GetType());
            
            var container = A.Fake<IContainer>();
            A.CallTo(() => container.Resolve(handler.GetType())).Returns(handler);

            var dispatcher = new MessageDispatcher(container, map);

            // Act.
            dispatcher.Dispatch("CreateOrder", new Envelope<CancelOrderCommand>(command)).Wait();

            // Assert.
            A.CallTo(() => handler.Handle(command)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.True(executed);
        }
        
        [Fact]
        public void DispatchShouldSupportMultipleHandlers()
        {
            // Arrange.
            var evt = new OrderCancelledEvent();

            var executed = 0;
            var handler1 = new OrderCancelledEventHandler1(() => { executed++; });
            var handler2 = new OrderCancelledEventHandler2(() => { executed++; });

            var map = new MessageHandlerMap();
            map.Add(typeof(OrderCancelledEvent), handler1.GetType());
            map.Add(typeof(OrderCancelledEvent), handler2.GetType());

            var container = A.Fake<IContainer>();
            A.CallTo(() => container.Resolve(handler1.GetType())).Returns(handler1);
            A.CallTo(() => container.Resolve(handler2.GetType())).Returns(handler2);

            var dispatcher = new MessageDispatcher(container, map);

            // Act.
            dispatcher.Dispatch("CreateOrder", new Envelope<OrderCancelledEvent>(evt)).Wait();

            // Assert.
            Assert.Equal(2, executed);
        }

        [Fact]
        public void DispatchShouldThrowIfHandlerNull()
        {
            // Arrange.
            var evt = new OrderCancelledEvent();
            var map = new MessageHandlerMap();
            map.Add(typeof(OrderCancelledEvent), typeof(OrderCancelledEventHandler1));

            var container = A.Fake<IContainer>();
            A.CallTo(() => container.Resolve(typeof(OrderCancelledEventHandler1))).Returns(null);

            // Act.
            Assert.Throws<NullReferenceException>(() =>
            {
                try
                {
                    var dispatcher = new MessageDispatcher(container, map);
                    dispatcher.Dispatch("CreateOrder", new Envelope<OrderCancelledEvent>(evt)).Wait();
                }
                catch (AggregateException ex)
                {
                    throw ex.InnerException;
                }
            });
        }

        [Fact]
        public void DispatchShouldSupportGiveAccessToEnvelope()
        {
            // Arrange.
            var evt = new OrderCancelledEvent();
            var handler = new EnvelopedOrderCancelledEventHandler();

            var map = new MessageHandlerMap();
            map.Add(typeof(OrderCancelledEvent), handler.GetType());

            var container = A.Fake<IContainer>();
            A.CallTo(() => container.Resolve(handler.GetType())).Returns(handler);

            var dispatcher = new MessageDispatcher(container, map);

            // Act.
            var envelope = new Envelope<OrderCancelledEvent>(evt);
            dispatcher.Dispatch("CreateOrder", envelope).Wait();

            // Assert.
            Assert.Equal(envelope, handler.Envelope);
        }
        [Fact]
        public void DispatchShouldSupportGiveAccessToSession()
        {
            // Arrange.
            var evt = new OrderCancelledEvent();
            var handler = new SessionOrderCancelledEventHandler();

            var map = new MessageHandlerMap();
            map.Add(typeof(OrderCancelledEvent), handler.GetType());

            var container = A.Fake<IContainer>();
            A.CallTo(() => container.Resolve(handler.GetType())).Returns(handler);

            var dispatcher = new MessageDispatcher(container, map);
            var session = A.Fake<ISession>();

            // Act.
            var envelope = new Envelope<OrderCancelledEvent>(evt);
            dispatcher.Dispatch("CreateOrder", envelope, session).Wait();

            // Assert.
            Assert.Equal(session, handler.Session);
        }
    }
}