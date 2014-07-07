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
    [EventSource(Name = "RedDog-Messaging")]
    public class MessagingEventSource : EventSource
    {
        public static readonly MessagingEventSource Log = new MessagingEventSource();

        public class Keywords
        {
            public const EventKeywords Configuration = (EventKeywords)1;
            public const EventKeywords Command = (EventKeywords)2;
            public const EventKeywords SessionCommand = (EventKeywords)4;
            public const EventKeywords Event = (EventKeywords)8;
            public const EventKeywords SessionEvent = (EventKeywords)16;
        }

        public class Tasks
        {
            public const EventTask Configuration = (EventTask)1;
            public const EventTask Send = (EventTask)2;
            public const EventTask Receive = (EventTask)3;
            public const EventTask Serialization = (EventTask)4;
            public const EventTask Deserialization = (EventTask)5;
            public const EventTask Processing = (EventTask)6;
        }

        #region Configuration
        [NonEvent]
        internal void RegisteredContainer(string configurationType, IContainer container)
        {
            if (IsEnabled())
                RegisteredContainer(configurationType, container.GetType().ToString(), Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(1000, Message = "{0}: container set to '{1}'.", Level = EventLevel.Informational, Task = Tasks.Configuration)]
        internal void RegisteredContainer(string configurationType, string containerType, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(1000, configurationType, containerType, threadId, taskId);
        }
        
        [NonEvent]
        internal void RegisteredSerializer(string configurationType, ISerializer serializer)
        {
            if (IsEnabled())
                RegisteredSerializer(configurationType, serializer.GetType().ToString(), Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(1001, Message = "{0}: serializer set to '{1}'.", Level = EventLevel.Informational, Task = Tasks.Configuration)]
        internal void RegisteredSerializer(string configurationType, string serializerType, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(1001, configurationType, serializerType, threadId, taskId);
        }
        
        [NonEvent]
        internal void RegisteredSerializationFilter(string configurationType, IMessageFilter filter)
        {
            if (IsEnabled())
                RegisteredSerializationFilter(configurationType, filter.GetType().ToString(), Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(1002, Message = "{0}: added serialization filter '{1}'.", Level = EventLevel.Informational, Task = Tasks.Configuration)]
        internal void RegisteredSerializationFilter(string configurationType, string filterType, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(1002, configurationType, filterType, threadId, taskId);
        }
        #endregion
        #region Processor Configuration
        [NonEvent]
        internal void RegisteredHandler(IMessageReceiver reciever, Type messageType, Type handlerType)
        {
            if (IsEnabled())
                RegisteredHandler(reciever.Path, messageType.Name, handlerType.Name, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(1050, Message = "Registered handler for {1}: {2}. ({0}).", Level = EventLevel.Informational, Task = Tasks.Configuration)]
        internal void RegisteredHandler(string entityPath, string messageType, string handlerType, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(1050, entityPath, messageType, handlerType,threadId, taskId);
        }
        [NonEvent]
        internal void RegisteredHandlerError(IMessageReceiver reciever, Type messageType, Exception exception)
        {
            if (IsEnabled())
                RegisteredHandlerError(reciever.Path, messageType.Name, exception.Message, exception.StackTrace, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(1051, Message = "Error while registering handler for {1}: {2}.", Level = EventLevel.Error, Task = Tasks.Configuration)]
        internal void RegisteredHandlerError(string entityPath, string messageType, string exceptionMessage, string exceptionStackTrace, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(1051, entityPath, messageType, exceptionMessage, exceptionStackTrace, threadId, taskId);
        }
        #endregion
        #region Bus Configuration
        [NonEvent]
        internal void RegisteredMessageSender(IMessageSender messageSender, Type messageType)
        {
            if (IsEnabled())
                RegisteredMessageSender(messageSender.GetType().Name, messageSender.Path, messageType.ToString());
        }

        [Event(1070, Message = "Registered {2} with destination: {0} ({1})", Level = EventLevel.Informational, Task = Tasks.Configuration)]
        internal void RegisteredMessageSender(string messageSenderType, string path, string messageType)
        {
            if (IsEnabled())
                WriteEvent(1070, messageSenderType, path, messageType);
        }
        #endregion
        #region Serialization
        [NonEvent]
        internal void SerializingMessage(IMessage message, IEnvelope envelope)
        {
            if (IsEnabled())
                SerializingMessage(message.GetType().Name, message.Id, envelope.CorrelationId, envelope.SessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(2050, Message = "Serializing message: {0}.", Level = EventLevel.Verbose, Task = Tasks.Serialization)]
        public void SerializingMessage(string messageType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(2050, messageType, messageId, correlationId, sessionId, threadId, taskId);
        }
        
        [NonEvent]
        internal void AfterSerialization(IMessageFilter filter, IEnvelope envelope)
        {
            if (IsEnabled())
                AfterSerialization(filter.GetType().Name, envelope.CorrelationId, envelope.SessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }
        
        [Event(2051, Message = "Executing serialization filter: {0}.", Level = EventLevel.Verbose, Task = Tasks.Serialization)]
        public void AfterSerialization(string filterType, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(2051, filterType, correlationId, sessionId, threadId, taskId);
        }
        
        [NonEvent]
        internal void SerializationComplete(IMessage message, IEnvelope envelope, int length)
        {
            if (IsEnabled())
                SerializationComplete(message.GetType().Name, length, message.Id, envelope.CorrelationId, envelope.SessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(2052, Message = "Serialized complete for {0}. Total size: {1} bytes.", Level = EventLevel.Verbose, Task = Tasks.Serialization)]
        public void SerializationComplete(string messageType, int length, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(2052, messageType, length, messageId, correlationId, sessionId, threadId, taskId);
        }

        [NonEvent]
        internal void DeserializingMessage(string contentType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                DeserializingMessage(contentType, messageId, correlationId, sessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(2060, Message = "Deserializing message: {0}.", Level = EventLevel.Verbose, Task = Tasks.Deserialization)]
        public void DeserializingMessage(string contentType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(2060, contentType, messageId, correlationId, sessionId, threadId, taskId);
        }

        [NonEvent]
        internal void BeforeDeserialization(string filterType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                BeforeDeserialization(filterType, messageId, correlationId, sessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }
        
        [Event(2061, Message = "Executing deserialization filter: {0}", Level = EventLevel.Verbose, Task = Tasks.Deserialization)]
        public void BeforeDeserialization(string filterType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(2061, filterType, messageId, correlationId, sessionId, threadId, taskId);
        }

        [NonEvent]
        internal void DeserializationComplete(string contentType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                DeserializationComplete(contentType, messageId, correlationId, sessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(2062, Message = "Deserialization complete for {0}.", Level = EventLevel.Verbose, Task = Tasks.Deserialization)]
        public void DeserializationComplete(string contentType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(2062, contentType, messageId, correlationId, sessionId, threadId, taskId);
        }
        #endregion
        #region Receive
        [NonEvent]
        internal void MessageProcessorStarting(string type, int receivers)
        {
            if (IsEnabled())
                MessageProcessorStarting(type, receivers, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4020, Message = "Starting {0} with {1} receivers.", Level = EventLevel.Informational, Task = Tasks.Receive)]
        public void MessageProcessorStarting(string type, int receivers, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4020, type, receivers, threadId, taskId);
        }

        [NonEvent]
        internal void MessageProcessorStartException(string type, Exception exception)
        {
            if (IsEnabled())
                MessageProcessorStartException(type, exception.Message, exception.StackTrace, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4021, Message = "Error starting {0}: {1}.", Level = EventLevel.Critical, Task = Tasks.Receive)]
        public void MessageProcessorStartException(string type, string exceptionMessage, string stacktrace, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4021, type, exceptionMessage, stacktrace, threadId, taskId);
        }

        [NonEvent]
        internal void MessageReceiverStarting(IMessageReceiver receiver, MessageHandlerMap map)
        {
            if (IsEnabled())
                MessageReceiverStarting(receiver.GetType().Name, receiver.Path, map.HandlerTypes.Count, map.HandlerTypes.SelectMany(m => m.Value).Count(), Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4022, Message = "Starting {0} on {1} for {2} message types and {3} handlers.", Level = EventLevel.Informational, Task = Tasks.Receive)]
        public void MessageReceiverStarting(string receiverType, string receiverPath, int messageTypes, int handlerTypes, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4022, receiverType, receiverPath, messageTypes, handlerTypes, threadId, taskId);
        }

        [NonEvent]
        internal void MessageReceived(string contentType, IMessageReceiver receiver, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                MessageReceived(contentType, receiver.GetType().Name, receiver.Path, messageId, correlationId, sessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4023, Message = "Received message {0} from {1} ({2}).", Level = EventLevel.Verbose, Task = Tasks.Receive)]
        public void MessageReceived(string contentType, string receiver, string path, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4023, contentType, receiver, path, messageId, correlationId, sessionId, threadId, taskId);
        }

        [NonEvent]
        public void MessageCompleting(string contentType, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                MessageCompleting(contentType, messageId, correlationId, sessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4024, Message = "Completing message {0}.", Level = EventLevel.Verbose, Task = Tasks.Receive)]
        public void MessageCompleting(string contentType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4024, contentType, messageId, correlationId, sessionId, threadId, taskId);
        }
        #endregion
        #region Processing
        [NonEvent]
        internal void MessageProcessing(Type bodyType, Type handlerType, IEnvelope envelope)
        {
            if (IsEnabled())
                MessageProcessing(bodyType.Name, handlerType.Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4051, Message = "Processing message {0} with handler: {1}.", Level = EventLevel.Informational, Task = Tasks.Processing)]
        public void MessageProcessing(string bodyType, string handlerType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4051, bodyType, handlerType, messageId, correlationId, sessionId, threadId, taskId);
        }

        [NonEvent]
        internal void MessageProcessed(Type bodyType, Type handlerType, IEnvelope envelope)
        {
            if (IsEnabled())
                MessageProcessed(bodyType.Name, handlerType.Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4052, Message = "Processed message {0} with handler: {1}.", Level = EventLevel.Verbose, Task = Tasks.Processing)]
        public void MessageProcessed(string bodyType, string handlerType, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4052, bodyType, handlerType, messageId, correlationId, sessionId, threadId, taskId);
        }

        [NonEvent]
        internal void MessageProcessingException(string contentType, IEnvelope envelope, Exception exception)
        {
            if (IsEnabled())
                MessageProcessingException(contentType, exception.Message, exception.StackTrace, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, Thread.CurrentThread.ManagedThreadId, GetTask());
        }

        [Event(4053, Message = "Error processing message {0}: {1}.", Level = EventLevel.Error, Task = Tasks.Processing)]
        internal void MessageProcessingException(string contentType, string exceptionMessage, string exceptionStackTrace, string messageId, string correlationId, string sessionId, int threadId, int taskId)
        {
            if (IsEnabled())
                WriteEvent(4053, contentType, exceptionMessage, exceptionStackTrace, threadId, taskId);
        }
        #endregion
        #region Send Command
        [NonEvent]
        public void SendingCommand(Type commandType, IEnvelope envelope)
        {
            if (IsEnabled())
                SendingCommand(commandType.Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2000, Message = "Sending command {0}.", Level = EventLevel.Informational, Keywords = Keywords.Command, Task = Tasks.Send)]
        internal void SendingCommand(string commandType, string messageId, string correlationId, string sessionId, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2000, commandType, messageId, correlationId, sessionId, taskId, threadId);
        }

        [NonEvent]
        public void CommandSent(Type commandType, IMessageSender sender, IEnvelope envelope)
        {
            if (IsEnabled())
                CommandSent(commandType.Name, sender.Path, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2001, Message = "Command {0} sent to: {1}", Level = EventLevel.Verbose, Keywords = Keywords.Command, Task = Tasks.Send)]
        internal void CommandSent(string commandType, string path, string messageId, string correlationId, string sessionId, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2001, commandType, path, messageId, correlationId, sessionId, taskId, threadId);
        }

        [NonEvent]
        public void CommandBatchSent(Type commandType, int messageCount, IMessageSender sender)
        {
            if (IsEnabled())
                CommandBatchSent(commandType.Name, messageCount, sender.Path, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2002, Message = "Command batch {0} ({1} commands) sent to: {2}", Level = EventLevel.Verbose, Keywords = Keywords.Command, Task = Tasks.Send)]
        internal void CommandBatchSent(string commandType, int messageCount, string path, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2002, commandType, messageCount, path, taskId, threadId);
        }

        [NonEvent]
        public void SendCommandFailed(Type commandType, Exception exception)
        {
            if (IsEnabled())
                SendCommandFailed(commandType.Name, exception.Message, exception.StackTrace, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2003, Message = "Error sending {0}: {1}", Level = EventLevel.Error, Keywords = Keywords.Command, Task = Tasks.Send)]
        internal void SendCommandFailed(string commandType, string exception, string stackTrace, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2003, commandType, exception, stackTrace, taskId, threadId);
        }

        [NonEvent]
        public void SendCommandBatchFailed(Type commandType, int totalCommands, Exception exception)
        {
            if (IsEnabled())
                SendCommandBatchFailed(commandType.Name, totalCommands, exception.Message, exception.StackTrace, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2004, Message = "Error sending {0} batch ({1} commands): {2}", Level = EventLevel.Error, Keywords = Keywords.Command, Task = Tasks.Send)]
        internal void SendCommandBatchFailed(string commandType, int totalCommands, string exception, string stackTrace, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2004, commandType, totalCommands, exception, stackTrace, taskId, threadId);
        }
        #endregion
        #region Send Event
        [NonEvent]
        public void PublishingEvent(Type eventType, IEnvelope envelope)
        {
            if (IsEnabled())
                PublishingEvent(eventType.Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2020, Message = "Publishing event {0}.", Level = EventLevel.Informational, Keywords = Keywords.Event, Task = Tasks.Send)]
        internal void PublishingEvent(string eventType, string messageId, string correlationId, string sessionId, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2020, eventType, messageId, correlationId, sessionId, taskId, threadId);
        }

        [NonEvent]
        public void EventPublished(Type eventType, IMessageSender sender, IEnvelope envelope)
        {
            if (IsEnabled())
                EventPublished(eventType.Name, sender.Path, envelope.MessageId, envelope.CorrelationId, envelope.SessionId, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2021, Message = "Event {0} published to: {1}", Level = EventLevel.Verbose, Keywords = Keywords.Event, Task = Tasks.Send)]
        internal void EventPublished(string eventType, string path, string messageId, string correlationId, string sessionId, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2021, eventType, path, messageId, correlationId, sessionId, taskId, threadId);
        }

        [NonEvent]
        public void EventBatchPublished(Type eventType, int messageCount, IMessageSender sender)
        {
            if (IsEnabled())
                EventBatchPublished(eventType.Name, messageCount, sender.Path, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2022, Message = "Event batch {0} ({1} events) published to: {2}", Level = EventLevel.Verbose, Keywords = Keywords.Event, Task = Tasks.Send)]
        internal void EventBatchPublished(string eventType, int messageCount, string path, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2022, eventType, messageCount, path, taskId, threadId);
        }

        [NonEvent]
        public void PublishEventFailed(Type eventType, Exception exception)
        {
            if (IsEnabled())
                PublishEventFailed(eventType.Name, exception.Message, exception.StackTrace, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2023, Message = "Error publishing {0}: {1}", Level = EventLevel.Error, Keywords = Keywords.Event, Task = Tasks.Send)]
        internal void PublishEventFailed(string eventType, string exception, string stackTrace, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2023, eventType, exception, stackTrace, taskId, threadId);
        }

        [NonEvent]
        public void PublishEventBatchFailed(Type eventType, int totalEvents, Exception exception)
        {
            if (IsEnabled())
                PublishEventBatchFailed(eventType.Name, totalEvents, exception.Message, exception.StackTrace, GetTask(), Thread.CurrentThread.ManagedThreadId);
        }

        [Event(2024, Message = "Error publishing {0} batch ({1} events): {2}", Level = EventLevel.Error, Keywords = Keywords.Event, Task = Tasks.Send)]
        internal void PublishEventBatchFailed(string eventType, int totalEvents, string exception, string stackTrace, int taskId, int threadId)
        {
            if (IsEnabled())
                WriteEvent(2024, eventType, totalEvents, exception, stackTrace, taskId, threadId);
        }
        #endregion

        private int GetTask()
        {
            return Task.CurrentId ?? 0;
        }
    }
}