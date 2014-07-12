using System;
using System.Diagnostics.Tracing;

namespace RedDog.ServiceBus.Diagnostics
{
    [EventSource(Name = "RedDog-ServiceBus")]
    public class ServiceBusEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords Queue = (EventKeywords)1;
            public const EventKeywords Topic = (EventKeywords)2;
            public const EventKeywords Subscription = (EventKeywords)4;
            public const EventKeywords Sender = (EventKeywords)8;
            public const EventKeywords Pump = (EventKeywords)16;
            public const EventKeywords Message = (EventKeywords)32;
            public const EventKeywords Session = (EventKeywords)64;
            public const EventKeywords SessionMessage = (EventKeywords)128;
        }

        public static readonly ServiceBusEventSource Log = new ServiceBusEventSource();

        #region Management
        [Event(1, Message = "Topic '{1}' created.", Level = EventLevel.Informational, Keywords = Keywords.Topic)]
        internal void CreatedTopic(string environment, string topic)
        {
            if (IsEnabled())
                WriteEvent(1, environment, topic);
        }

        [Event(2, Message = "Subscription '{2}' created for '{1}'.", Level = EventLevel.Informational, Keywords = Keywords.Subscription)]
        internal void CreatedSubscription(string environment, string topic, string subscription)
        {
            if (IsEnabled())
                WriteEvent(2, environment, topic, subscription);
        }

        [Event(3, Message = "Queue '{1}' created.", Level = EventLevel.Informational, Keywords = Keywords.Queue)]
        internal void CreatedQueue(string environment, string queue)
        {
            if (IsEnabled())
                WriteEvent(3, environment, queue);
        }
        #endregion
        #region Send
        [Event(100, Message = "Sending message to '{1}'. M='{2}', C='{3}', S='{4}'", Level = EventLevel.Verbose, Keywords = Keywords.Message)]
        internal void SendMessage(string environment, string queue, string messageId, string correlationId, string sessionId)
        {
            if (IsEnabled())
                WriteEvent(100, environment, queue, messageId, correlationId, sessionId);
        }

        [Event(101, Message = "Message sent to '{1}' in '{5}s'", Level = EventLevel.Verbose, Keywords = Keywords.Message)]
        internal void SentMessage(string environment, string queue, string messageId, string correlationId, string sessionId, double durationSeconds)
        {
            if (IsEnabled())
                WriteEvent(101, environment, queue, messageId, correlationId, sessionId, Math.Round(durationSeconds, 3));
        }

        [Event(150, Message = "Sending batch of {2} messages to '{1}'.", Level = EventLevel.Verbose, Keywords = Keywords.Message)]
        internal void SendMessageBatch(string environment, string queue, int messageCount)
        {
            if (IsEnabled())
                WriteEvent(150, environment, queue, messageCount);
        }

        [Event(151, Message = "Batch with {2} messages sent to '{1}' in '{3}s'", Level = EventLevel.Verbose, Keywords = Keywords.Message)]
        internal void SentMessageBatch(string environment, string queue, int messageCount, double durationSeconds)
        {
            if (IsEnabled())
                WriteEvent(151, environment, queue, messageCount, Math.Round(durationSeconds, 3));
        }
        #endregion
        #region Session Message Pump
        [NonEvent]
        internal void SessionMessagePumpStart(string messagePumpType, string environment, string path, TimeSpan autoRenewSessionTimeout, int maxConcurrentSessions)
        {
            if (IsEnabled())
            {
                SessionMessagePumpStart(messagePumpType, environment, path, autoRenewSessionTimeout.ToString(), maxConcurrentSessions);
            }
        }

        [Event(200, Level = EventLevel.Informational, Keywords = Keywords.Pump, Message = "Starting '{0}' on '{2}' with 'AutoRenewSessionTimeout={3}', 'MaxConcurrentSessions={4}'.")]

        internal void SessionMessagePumpStart(string messagePumpType, string environment, string path, string autoRenewSessionTimeout, int maxConcurrentSessions)
        {
            if (IsEnabled())
                WriteEvent(200, messagePumpType, environment, path, autoRenewSessionTimeout, maxConcurrentSessions);
        }

        [Event(201, Level = EventLevel.Verbose, Keywords = Keywords.Session, Message = "Session '{2}' accepted on '{1}'")]
        internal void SessonAccepted(string environment, string path, string sessionId, string messageId, string correlationId)
        {
            if (IsEnabled())
                WriteEvent(201, environment, path, sessionId);
        }

        [Event(202, Level = EventLevel.Verbose, Keywords = Keywords.SessionMessage, Message = "Received message from '{1}'. MsgId={2}, CorrId={3}, SessId={4}, Deliveries={5}, Size={6}")]
        internal void SessionMessageReceived(string environment, string path, string messageId, string correlationId, string sessionId, int deliveryCount, long size)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";
                if (String.IsNullOrEmpty(sessionId))
                    sessionId = "n/a";

                WriteEvent(202, environment, path, messageId, correlationId, sessionId, deliveryCount, size);
            }
        }

        [Event(203, Level = EventLevel.Verbose, Keywords = Keywords.Session, Message = "Session '{2}' on '{1}' was closed.")]
        internal void SessionClosed(string environment, string path, string sessionId)
        {
            if (IsEnabled())
                WriteEvent(203, environment, path, sessionId);
        }
        
        [NonEvent]
        internal void SessionLost(string environment, string path, string sessionId, Exception exception)
        {
            if (IsEnabled())
            {
                SessionLost(environment, path, sessionId, exception.GetType().Name, exception.Message, exception.StackTrace);
            }
        }

        [Event(204, Level = EventLevel.Error, Keywords = Keywords.Pump, Message = "Session '{2}' on '{1}' was lost: '{4}'")]
        internal void SessionLost(string environment, string path, string sessionId, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(204, environment, path, sessionId, exceptionType, exceptionMessage, stackTrace);
        }
        
        [Event(205, Level = EventLevel.Verbose, Keywords = Keywords.Session, Message = "Session handler for '{2}' on '{1}' was disposed.")]
        internal void SessionHandlerDisposed(string environment, string path, string sessionId)
        {
            if (IsEnabled())
                WriteEvent(205, environment, path, sessionId);
        }
        #endregion
        #region Message Pump
        [NonEvent]
        internal void MessagePumpStart(string messagePumpType, string environment, string path, TimeSpan autoRenewTimeout, int maxConcurrentCalls)
        {
            if (IsEnabled())
            {
                MessagePumpStart(messagePumpType, environment, path, autoRenewTimeout.ToString(), maxConcurrentCalls);
            }
        }

        [Event(300, Level = EventLevel.Informational, Keywords = Keywords.Pump, Message = "Starting '{0}' on '{2}' with 'AutoRenewTimeout={3}', 'MaxConcurrentCalls={4}'.")]

        internal void MessagePumpStart(string messagePumpType, string environment, string path, string autoRenewTimeout, int maxConcurrentCalls)
        {
            if (IsEnabled())
                WriteEvent(300, messagePumpType, environment, path, autoRenewTimeout, maxConcurrentCalls);
        }

        [Event(301, Level = EventLevel.Verbose, Keywords = Keywords.Message, Message = "Received message from '{1}'. MsgId={2}, CorrId={3}, Deliveries={4}, Size={5}")]
        internal void MessageReceived(string environment, string path, string messageId, string correlationId, int deliveryCount, long size)
        {
            if (IsEnabled())
            {
                if (String.IsNullOrEmpty(messageId))
                    messageId = "n/a";
                if (String.IsNullOrEmpty(correlationId))
                    correlationId = "n/a";

                WriteEvent(301, environment, path, messageId, correlationId, deliveryCount, size);
            }
        }

        [NonEvent]
        internal void MessagePumpExceptionReceived(string environment, string path, string action, Exception exception)
        {
            if (IsEnabled())
            {
                MessagePumpExceptionReceived(environment, path, action, exception.GetType().Name, exception.Message, exception.StackTrace);
            }
        }

        [Event(302, Level = EventLevel.Error, Keywords = Keywords.Pump, Message = "'{3}' received on '{1}': {4}")]
        internal void MessagePumpExceptionReceived(string environment, string path, string action, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(302, environment, path, action, exceptionType, exceptionMessage, stackTrace);
        }
        #endregion
    }
}