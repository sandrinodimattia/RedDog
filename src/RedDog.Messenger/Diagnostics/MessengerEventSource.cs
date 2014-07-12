using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;

using RedDog.Messenger.Composition;
using RedDog.Messenger.Contracts;
using RedDog.Messenger.Filters;
using RedDog.Messenger.Processor;
using RedDog.Messenger.Serialization;

using RedDog.ServiceBus.Receive;
using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Diagnostics
{
    [EventSource(Name = "RedDog-Messenger")]
    public class MessengerEventSource : EventSource
    {
        public static readonly MessengerEventSource Log = new MessengerEventSource();

        public class Keywords
        {
            public const EventKeywords Configuration = (EventKeywords)1;
            public const EventKeywords Serialization = (EventKeywords)2;
            public const EventKeywords Receive = (EventKeywords)4;
            public const EventKeywords Dispatch = (EventKeywords)8;
            public const EventKeywords Send = (EventKeywords)16;
        }

        #region Configuration
        [NonEvent]
        internal void RegisteredContainer(string configurationType, IContainer container)
        {
            if (IsEnabled())
                RegisteredContainer(configurationType, container.GetType().Name);
        }

        [Event(1000, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Message = "{0} configuration: 'Container={1}'.")]
        internal void RegisteredContainer(string configurationType, string containerType)
        {
            if (IsEnabled())
                WriteEvent(1000, configurationType, containerType);
        }
        
        [NonEvent]
        internal void RegisteredSerializer(string configurationType, ISerializer serializer)
        {
            if (IsEnabled())
                RegisteredSerializer(configurationType, serializer.GetType().Name);
        }

        [Event(1001, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Message = "{0} configuration: 'Serializer={1}'.")]
        internal void RegisteredSerializer(string configurationType, string serializerType)
        {
            if (IsEnabled())
                WriteEvent(1001, configurationType, serializerType);
        }
        
        [NonEvent]
        internal void RegisteredSerializationFilter(string configurationType, IMessageFilter filter)
        {
            if (IsEnabled())
                RegisteredSerializationFilter(configurationType, filter.GetType().Name);
        }

        [Event(1002, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Message = "{0} configuration: 'SerializationFilter={1}'.")]
        internal void RegisteredSerializationFilter(string configurationType, string filterType)
        {
            if (IsEnabled())
                WriteEvent(1002, configurationType, filterType);
        }
        #endregion
        #region Processor Configuration
        [NonEvent]
        internal void RegisteredHandler(IMessageReceiver reciever, Type messageType, Type handlerType)
        {
            if (IsEnabled())
                RegisteredHandler(reciever.Path, messageType.Name, handlerType.Name);
        }

        [Event(1050, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Message = "Processor: Handler registered on '{0}': '{1}' -> '{2}'.")]
        internal void RegisteredHandler(string entityPath, string messageType, string handlerType)
        {
            if (IsEnabled())
                WriteEvent(1050, entityPath, messageType, handlerType);
        }
        [NonEvent]
        internal void RegisteredHandlerError(IMessageReceiver reciever, Type messageType, Exception exception)
        {
            if (IsEnabled())
                RegisteredHandlerError(reciever.Path, messageType.Name, exception.GetType().Name, exception.Message, exception.StackTrace);
        }

        [Event(1051, Level = EventLevel.Error, Keywords = Keywords.Configuration, Message = "Processor: Handler registration '{2}': {3}.")]
        internal void RegisteredHandlerError(string entityPath, string messageType, string exceptionType, string exceptionMessage, string exceptionStackTrace)
        {
            if (IsEnabled())
                WriteEvent(1051, entityPath, messageType, exceptionType, exceptionMessage, exceptionStackTrace);
        }
        #endregion
        #region Bus Configuration
        [NonEvent]
        internal void RegisteredMessageSender(IMessageSender messageSender, Type messageType)
        {
            if (IsEnabled())
                RegisteredMessageSender(messageSender.Path, messageSender.GetType().Name, messageType.ToString());
        }

        [Event(1070, Message = "Bus: Message '{2}' registered with '{0}'", Level = EventLevel.Informational, Keywords = Keywords.Configuration)]
        internal void RegisteredMessageSender(string path, string messageSenderType, string messageType)
        {
            if (IsEnabled())
                WriteEvent(1070, path, messageSenderType, messageType);
        }
        #endregion
        #region Serialization
        [NonEvent]
        internal void SerializingMessage(IMessage message, IEnvelope envelope)
        {
            if (IsEnabled())
                SerializingMessage(message.GetType().Name, message.Id, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(2050, Message = "Serializing '{0}'. MsgId={1}, CorrId={2}, SessId={3}", Level = EventLevel.Verbose, Keywords = Keywords.Serialization)]
        public void SerializingMessage(string messageType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2050, messageType, messageId, correlationId, sessionId);
            }
        }
        
        [NonEvent]
        internal void AfterSerialization(IMessageFilter filter, IEnvelope envelope)
        {
            if (IsEnabled())
                AfterSerialization(filter.GetType().Name, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(2051, Message = "Executing '{0}'. MsgId=n/a, CorrId={1}, SessId={2}", Level = EventLevel.Verbose, Keywords = Keywords.Serialization)]
        public void AfterSerialization(string filterType, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                WriteEvent(2051, filterType, correlationId, sessionId);
            }
        }
        
        [NonEvent]
        internal void SerializationComplete(IMessage message, IEnvelope envelope, int length)
        {
            if (IsEnabled())
                SerializationComplete(message.GetType().Name, length, message.Id, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(2052, Message = "Serialized '{0}' to {1} bytes. . MsgId={2}, CorrId={3}, SessId={4}", Level = EventLevel.Verbose, Keywords = Keywords.Serialization)]
        public void SerializationComplete(string messageType, int length, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2052, messageType, length, messageId, correlationId, sessionId);
            }
        }

        [Event(2060, Message = "Deserializing '{0}'. MsgId={1}, CorrId={2}, SessId={3}", Level = EventLevel.Verbose, Keywords = Keywords.Serialization)]
        public void DeserializingMessage(string contentType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2060, contentType, messageId, correlationId, sessionId);
            }
        }

        [Event(2061, Message = "Executing '{0}'. MsgId={1}, CorrId={2}, SessId={3}", Level = EventLevel.Verbose, Keywords = Keywords.Serialization)]
        public void BeforeDeserialization(string filterType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2061, filterType, messageId, correlationId, sessionId);
            }
        }

        [Event(2062, Message = "Deserialized '{0}'. MsgId={1}, CorrId={2}, SessId={3}", Level = EventLevel.Verbose, Keywords = Keywords.Serialization)]
        public void DeserializationComplete(string contentType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2062, contentType, messageId, correlationId, sessionId);
            }
        }
        #endregion
        #region Receive
        [Event(4020, Message = "Processor: Starting '{0}' with '{1} receivers'.", Level = EventLevel.Informational, Keywords = Keywords.Receive)]
        public void MessageProcessorStarting(string type, int receivers)
        {
            if (IsEnabled())
                WriteEvent(4020, type, receivers);
        }

        [NonEvent]
        internal void MessageProcessorStartException(string type, Exception exception)
        {
            if (IsEnabled())
                MessageProcessorStartException(type, exception.GetType().Name, exception.Message, exception.StackTrace);
        }

        [Event(4021, Message = "Processor: '{1}' when starting '{0}', {2}.", Level = EventLevel.Critical, Keywords = Keywords.Receive)]
        public void MessageProcessorStartException(string type, string exceptionType, string exceptionMessage, string stacktrace)
        {
            if (IsEnabled())
                WriteEvent(4021, type, exceptionType, exceptionMessage, stacktrace);
        }

        [NonEvent]
        internal void MessageReceiverStarting(IMessageReceiver receiver, MessageHandlerMap map)
        {
            if (IsEnabled())
                MessageReceiverStarting(receiver.GetType().Name, receiver.Path, map.HandlerTypes.Count, map.HandlerTypes.SelectMany(m => m.Value).Count());
        }

        [Event(4022, Message = "Processor: Starting '{0}' on '{1}'. MessageTypes={2}, Handlers={3}.", Level = EventLevel.Informational, Keywords = Keywords.Receive)]
        public void MessageReceiverStarting(string receiverType, string receiverPath, int messageTypes, int handlerTypes)
        {
            if (IsEnabled())
                WriteEvent(4022, receiverType, receiverPath, messageTypes, handlerTypes);
        }

        [NonEvent]
        internal void MessageReceived(string contentType, IMessageReceiver receiver, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                MessageReceived(contentType, receiver.GetType().Name, receiver.Path, messageId, correlationId, sessionId);
        }

        [Event(4023, Message = "Processor: Received '{0}' from '{2}'. MsgId={3}, CorrId={4}, SessId={5}", Level = EventLevel.Verbose, Keywords = Keywords.Receive)]
        public void MessageReceived(string contentType, string receiver, string path, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(4023, contentType, receiver, path, messageId, correlationId, sessionId);
            }
        }

        [Event(4024, Message = "Processor: Completing message '{0}'. MsgId={1}, CorrId={2}, SessId={3}", Level = EventLevel.Verbose, Keywords = Keywords.Receive)]
        public void MessageCompleting(string contentType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(4024, contentType, messageId, correlationId, sessionId);
            }
        }
        #endregion
        #region Processing
        [NonEvent]
        internal void MessageProcessing(Type bodyType, Type handlerType, IEnvelope envelope)
        {
            if (IsEnabled())
                MessageProcessing(bodyType.Name, handlerType.Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(4051, Message = "Dispatcher: Processing '{0}' with '{1}'. MsgId={2}, CorrId={3}, SessId={4}", Level = EventLevel.Informational, Keywords = Keywords.Dispatch)]
        public void MessageProcessing(string bodyType, string handlerType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(4051, bodyType, handlerType, messageId, correlationId, sessionId);
            }
        }

        [NonEvent]
        internal void MessageProcessed(Type bodyType, Type handlerType, double totalSeconds, IEnvelope envelope)
        {
            if (IsEnabled())
                MessageProcessed(bodyType.Name, handlerType.Name, totalSeconds, envelope.MessageId, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(4052, Message = "Dispatcher: Processed '{0}' with '{1}' in '{2}s'. MsgId={3}, CorrId={4}, SessId={5}", Level = EventLevel.Verbose, Keywords = Keywords.Dispatch)]
        public void MessageProcessed(string bodyType, string handlerType, double totalSeconds, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(4052, bodyType, handlerType, totalSeconds, messageId, correlationId, sessionId);
            }
        }

        [NonEvent]
        internal void MessageProcessingException(string contentType, IEnvelope envelope, Exception exception)
        {
            if (IsEnabled())
                MessageProcessingException(contentType, exception.GetType().Name, exception.Message, exception.StackTrace, envelope.MessageId, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(4053, Message = "Dispatcher: '{1}' while processing '{0}', {2}. MsgId={4}, CorrId={5}, SessId={6}", Level = EventLevel.Error, Keywords = Keywords.Dispatch)]
        internal void MessageProcessingException(string contentType, string exceptionType, string exceptionMessage, string exceptionStackTrace, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(4053, contentType, exceptionType, exceptionMessage, exceptionStackTrace, messageId, correlationId, sessionId);
            }
        }
        #endregion
        #region Send 
        [NonEvent]
        public void Sending(Type messageType, IEnvelope envelope)
        {
            if (IsEnabled())
                Sending(messageType.Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(2000, Message = "Bus: Sending '{0}'. MsgId={1}, CorrId={2}, SessId={3}", Level = EventLevel.Informational, Keywords = Keywords.Send)]
        internal void Sending(string messageType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2000, messageType, messageId, correlationId, sessionId);
            }
        }

        [NonEvent]
        public void Sent(Type messageType, IMessageSender sender, IEnvelope envelope)
        {
            if (IsEnabled())
                Sent(messageType.Name, sender.Path, envelope.MessageId, envelope.CorrelationId, envelope.SessionId);
        }

        [Event(2001, Message = "Bus: Message '{0}' sent to '{1}'.  MsgId={2}, CorrId={3}, SessId={4}", Level = EventLevel.Verbose, Keywords = Keywords.Send)]
        internal void Sent(string messageType, string path, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";
                WriteEvent(2001, messageType, path, messageId, correlationId, sessionId);
            }
        }

        [NonEvent]
        public void BatchSent(Type messageType, int messageCount, IMessageSender sender)
        {
            if (IsEnabled())
                BatchSent(messageType.Name, messageCount, sender.Path);
        }

        [Event(2002, Message = "Bus: Batch of '{1} {0}' sent to {2}.", Level = EventLevel.Verbose, Keywords = Keywords.Send)]
        internal void BatchSent(string messageType, int messageCount, string path)
        {
            if (IsEnabled())
                WriteEvent(2002, messageType, messageCount, path);
        }

        [NonEvent]
        public void SendFailed(Type messageType, Exception exception)
        {
            if (IsEnabled())
                SendFailed(messageType.Name, exception.GetType().Name, exception.Message, exception.StackTrace);
        }

        [Event(2003, Message = "Bus: '{1}' when sending '{0}', {2}", Level = EventLevel.Error, Keywords = Keywords.Send)]
        internal void SendFailed(string messageType, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(2003, messageType, exceptionType, exceptionMessage, stackTrace);
        }

        [NonEvent]
        public void SendBatchFailed(Type messageType, int totals, Exception exception)
        {
            if (IsEnabled())
                SendBatchFailed(messageType.Name, totals, exception.GetType().Name, exception.Message, exception.StackTrace);
        }

        [Event(2004, Message = "Bus: '{2}' when sending '{0}' batch, {3}.", Level = EventLevel.Error, Keywords = Keywords.Send)]
        internal void SendBatchFailed(string messageType, int totals, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(2004, messageType, totals, exceptionType, exceptionMessage, stackTrace);
        }
        #endregion
    }
}