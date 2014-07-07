using RedDog.Messenger.Filters.Serialization;
using RedDog.Messenger.Serialization.Json;
using RedDog.Messenger.Tests.Bus.Commands;
using Xunit;

namespace RedDog.Messenger.Tests.Filters.Serialization
{
    public class GzipCompressionMessageFilterFacts
    {
        [Fact]
        public void SerializationShouldBeCompressed()
        {
            // Arrange.
            var filter = new GzipCompressionMessageFilter();
            var envelope = Envelope.Create(new CreateOrderCommand { Id = "abc" })
                .Property("Something", 123);
            var serializer = new NewtonsoftJsonSerializer();
            var serializedBody = serializer.Serialize(envelope.Body).Result;

            // Act.
            var compressedBody = filter.AfterSerialization(envelope, serializedBody).Result;

            // Assert.
            Assert.True(compressedBody.Length < serializedBody.Length);
        }

        [Fact]
        public void DeserializationShouldWorkCorrectly()
        {
            // Arrange.
            var filter = new GzipCompressionMessageFilter();
            var envelope = Envelope.Create(new CreateOrderCommand { Id = "abc" })
                .Property("Something", 123);
            var serializer = new NewtonsoftJsonSerializer();
            var serializedBody = serializer.Serialize(envelope.Body).Result;
            var compressedBody = filter.AfterSerialization(envelope, serializedBody).Result;

            // Act.
            var decompressedBody = filter.BeforeDeserialization(envelope, compressedBody).Result;
            var command = serializer.Deserialize<CreateOrderCommand>(decompressedBody).Result;

            // Assert.
            Assert.Equal(envelope.Body.Id, command.Id);
        }
    }
}
