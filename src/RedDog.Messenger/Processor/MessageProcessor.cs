using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.Messenger.Configuration;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Diagnostics;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Receive.Session;

namespace RedDog.Messenger.Processor
{
    public abstract class MessageProcessor : IProcessor
    {
        protected IProcessorConfiguration<IMessagingConfiguration> Configuration
        {
            get;
            private set;
        }

        protected MessageProcessor(IProcessorConfiguration<IMessagingConfiguration> configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Convert the brokered message to an envelope.
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns></returns>
        public async Task<IEnvelope<IMessage>> BuildMessage(BrokeredMessage brokeredMessage)
        {
            using (var stream = brokeredMessage.GetBody<Stream>())
            using (var ms = new MemoryStream())
            {
                var messageType = brokeredMessage.ContentType;

                // Log.
                MessengerEventSource.Log.DeserializingMessage(messageType, brokeredMessage.MessageId, brokeredMessage.CorrelationId, brokeredMessage.SessionId);

                // Helps us get access to the byte array.
                await stream.CopyToAsync(ms)
                    .ConfigureAwait(false);

                // Build the envelope.
                var envelope = Envelope.Create<IMessage>(null)
                    .CorrelationId(brokeredMessage.CorrelationId)
                    .SessionId(brokeredMessage.SessionId)
                    .TimeToLive(brokeredMessage.TimeToLive)
                    .Properties(brokeredMessage.Properties);

                // Handle interceptors, then deserialize.
                var serializedMessage = await Configuration.MessageFilterInvoker.BeforeDeserialization(envelope, ms.ToArray())
                    .ConfigureAwait(false);
                var message = await Configuration.Serializer.Deserialize<IMessage>(serializedMessage)
                    .ConfigureAwait(false);

                // Log.
                MessengerEventSource.Log.DeserializationComplete(messageType, brokeredMessage.MessageId, brokeredMessage.CorrelationId, brokeredMessage.SessionId);

                // Done.
                return envelope.Body(message);
            }
        }
        
        /// <summary>
        /// Start the processor.
        /// </summary>
        public void Start()
        {
            MessengerEventSource.Log.MessageProcessorStarting(GetType().Name, Configuration.Receivers.Count);

            try
            {
                // Start each receiver.
                foreach (var receiver in Configuration.Receivers)
                {
                    if (receiver.Key is IMessagePump)
                    {
                        StartReceiver(receiver.Key as IMessagePump, receiver.Value);
                    }
                    else if (receiver.Key is ISessionMessagePump)
                    {
                        StartSessionReceiver(receiver.Key as ISessionMessagePump, receiver.Value);
                    }
                    else
                    {
                        throw new ProcessorConfigurationException("Invalid receiver type: {0}",
                            receiver.Key.GetType().Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessengerEventSource.Log.MessageProcessorStartException(GetType().Name, ex);

                // Rethrow.
                throw;
            }
        }

        /// <summary>
        /// Start the receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="handlerMap"></param>
        private void StartReceiver(IMessagePump receiver, MessageHandlerMap handlerMap)
        {
            MessengerEventSource.Log.MessageReceiverStarting(receiver, handlerMap);

            receiver.StartAsync(message => OnHandleMessage(receiver, null, message, handlerMap), OnError).Wait();
        }

        /// <summary>
        /// Start the session receiver.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="handlerMap"></param>
        private void StartSessionReceiver(ISessionMessagePump receiver, MessageHandlerMap handlerMap)
        {
            MessengerEventSource.Log.MessageReceiverStarting(receiver, handlerMap);

            receiver.StartAsync((session, message) => OnHandleMessage(receiver, session, message, handlerMap), OnError).Wait();
        }

        /// <summary>
        /// Handle exceptions.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Task OnError(string action, Exception exception)
        {
            if (Configuration.ErrorHandler != null)
                return Configuration.ErrorHandler(action, exception);
            return Task.FromResult(false);
        }

        /// <summary>
        /// Handle the message.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="brokeredMessage"></param>
        /// <param name="receiver"></param>
        /// <param name="handlerMap"></param>
        /// <returns></returns>
        private async Task OnHandleMessage(IMessageReceiver receiver, MessageSession session, BrokeredMessage brokeredMessage, MessageHandlerMap handlerMap)
        {
            MessengerEventSource.Log.MessageReceived(brokeredMessage.ContentType, receiver, brokeredMessage.MessageId, brokeredMessage.CorrelationId, brokeredMessage.SessionId);

            // Create new isolated scope.
            using (var scope = Configuration.Container.BeginScope())
            {
                var envelope = await BuildMessage(brokeredMessage)
                    .ConfigureAwait(false);

                // Dispatch the message.
                var dispatcher = new MessageDispatcher(scope, handlerMap);
                await dispatcher.Dispatch(brokeredMessage.ContentType, envelope, session != null ? new Session(session, Configuration.Serializer) : null)
                    .ConfigureAwait(false);
            }
        }
    }
}