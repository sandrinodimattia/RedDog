using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.Messenger.Configuration;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Diagnostics;
using RedDog.Messenger.Utils;

namespace RedDog.Messenger.Bus
{
    public abstract class MessageBus
    {
        protected IBusConfiguration<IMessagingConfiguration> Configuration
        {
            get;
            private set;
        }

        protected MessageBus(IBusConfiguration<IMessagingConfiguration> configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Convert an envelope to a brokered message.
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        protected async Task<BrokeredMessage> BuildBrokeredMessage(IEnvelope<IMessage> envelope)
        {
            MemoryStream stream = null;

            try
            {
                // Log.
                MessagingEventSource.Log.SerializingMessage(envelope.Body, envelope);

                // Serialize the message and notify the interceptor.
                byte[] serializedMessage = await Configuration.Serializer.Serialize(envelope.Body).ConfigureAwait(false);
                serializedMessage = await Configuration.MessageFilterInvoker.AfterSerialization(envelope, serializedMessage).ConfigureAwait(false);

                // Log.
                MessagingEventSource.Log.SerializationComplete(envelope.Body, envelope, serializedMessage.Length);
                
                // Create a message containing the stream and the type.
                stream = new MemoryStream(serializedMessage);
                var message = new BrokeredMessage(stream)
                {
                    ContentType = String.Format("{0}, {1}", envelope.Body.GetType().Name,
                        String.Join(",", envelope.Body.GetType().Assembly.FullName.Split(',').Take(1)))
                };

                // Set the values on the message depending on what has been set on the envelope.
                message.CorrelationId = envelope.CorrelationId.NotEmpty() ? envelope.CorrelationId : message.MessageId;
                if (envelope.Body.Id.NotEmpty())
                    message.MessageId = envelope.Body.Id;
                if (envelope.Delay > TimeSpan.Zero)
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(envelope.Delay);
                if (envelope.TimeToLive > TimeSpan.Zero)
                    message.TimeToLive = envelope.TimeToLive;
                if (envelope.SessionId.NotEmpty())
                    message.SessionId = envelope.SessionId;

                // Copy properties.
                if (envelope.Properties != null && envelope.Properties.Count > 0)
                {
                    foreach (var prop in envelope.Properties)
                        message.Properties.Add(prop.Key, prop.Value);
                }

                return message;
            }
            catch
            {
                if (stream != null)
                    stream.Dispose();
                throw;
            }
        }
    }
}