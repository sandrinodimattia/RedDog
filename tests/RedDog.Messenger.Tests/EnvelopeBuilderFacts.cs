using System;
using System.Collections.Generic;
using RedDog.Messenger.Tests.Bus.Commands;
using Xunit;

namespace RedDog.Messenger.Tests
{
    public class EnvelopeBuilderFacts
    {
        [Fact]
        public void CreateShouldFillInAllProperties()
        {
            // Act.
            var envelope = Envelope.Create(new CreateOrderCommand { Id = "123" }, "789", "session1")
                .Delayed(TimeSpan.FromMinutes(5))
                .Property("MyKey", "MyValue")
                .TimeToLive(TimeSpan.FromMinutes(5));

            // Assert.
            Assert.Equal("123", envelope.Body.Id);
            Assert.Equal("789", envelope.CorrelationId);
            Assert.Equal("session1", envelope.SessionId);
            Assert.Equal(TimeSpan.FromMinutes(5), envelope.Delay);
            Assert.Equal(TimeSpan.FromMinutes(5), envelope.TimeToLive);
            Assert.Equal("MyValue", envelope.Properties["MyKey"]);
        }

        [Fact]
        public void CreateShouldUseMessageIdForCorrelationIfCorrelationIdNotSet()
        {
            // Act.
            var envelope = Envelope.Create(new CreateOrderCommand { Id = "123" });

            // Assert.
            Assert.Equal("123", envelope.CorrelationId);
        }

        [Fact]
        public void CreateShouldGenerateId()
        {
            // Act.
            var envelope = Envelope.Create(new CreateOrderCommand { }, "789", "session1");

            // Assert.
            Assert.NotEmpty(envelope.Body.Id);
        }

        [Fact]
        public void CreateShouldAllowEmptyBody()
        {
            // Act.
            var envelope = Envelope.Create<CreateOrderCommand>(null, "789", "session1");

            // Assert.
            Assert.NotNull(envelope);
        }

        [Fact]
        public void CreateShouldSupportExtensionMethods()
        {
            // Act.
            var envelope = Envelope.Create<CreateOrderCommand>(null)
                .Body(new CreateOrderCommand { })
                .CorrelationId("cor")
                .SessionId("ses");

            // Assert.
            Assert.NotNull(envelope.Body);
            Assert.Equal("cor", envelope.CorrelationId);
            Assert.Equal("ses", envelope.SessionId);
        }

        [Fact]
        public void CreateShouldSupportDictionaries()
        {
            // Arrange.
            var d = new Dictionary<string, object>
            {
                {"first", 1}, 
                {"second", 2}
            };

            // Act.
            var envelope = Envelope.Create(new CreateOrderCommand {})
                .Properties(d);

            // Assert.
            Assert.Equal(1, envelope.Properties["first"]);
            Assert.Equal(2, envelope.Properties["second"]);
        }
    }
}
