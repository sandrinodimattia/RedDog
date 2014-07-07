using System;
using System.Threading.Tasks;

using RedDog.Messenger.Composition;

using RedDog.Messenger.Contracts;
using RedDog.Messenger.Contracts.Handlers;

using RedDog.Messenger.Diagnostics;

namespace RedDog.Messenger.Processor
{
    public class MessageDispatcher
    {
        private readonly IContainer _container;

        private readonly MessageHandlerMap _handlerMap;

        public MessageDispatcher(IContainer container, MessageHandlerMap handlerMap)
        {
            _container = container;
            _handlerMap = handlerMap;
        }

        public async Task Dispatch(string contentType, IEnvelope<IMessage> envelope, ISession session = null)
        {

            try
            {
                if (envelope.Body == null)
                    throw new MessageDispatcherException("The envelope did not contain a body. Unable to process message.");

                var bodyType = envelope.Body.GetType();

                // Get the handler types.
                if (!_handlerMap.HandlerTypes.ContainsKey(bodyType))
                {
                    throw new MessageDispatcherException("The message {0} has not been registered with a handler.", bodyType.Name);
                }

                // Execute handlers.
                foreach (var handlerType in _handlerMap.HandlerTypes[bodyType])
                {
                    // Log start.
                    MessagingEventSource.Log.MessageProcessing(bodyType, handlerType, envelope);

                    // Create the handler.
                    var handler = _container.Resolve(handlerType) as IMessageHandler;
                    if (handler == null)
                        throw new NullReferenceException("handler");

                    // Set envelope.
                    var envelopedHandler = handler as IEnvelopedHandler;
                    if (envelopedHandler != null)
                    {
                        envelopedHandler.Envelope = envelope;
                    }

                    // Set session.
                    var sessionHandler = handler as ISessionHandler;
                    if (sessionHandler != null)
                    {
                        sessionHandler.Session = session;
                    }

                    // Execute.
                    await ((handler as dynamic).Handle((dynamic)(envelope.Body)) as Task);

                    // Log end.
                    MessagingEventSource.Log.MessageProcessed(bodyType, handlerType, envelope);
                }
            }
            catch (Exception ex)
            {
                // Log.
                MessagingEventSource.Log.MessageProcessingException(contentType, envelope, ex);

                // Rethrow.
                throw;
            }
        }
    }
}