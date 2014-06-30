using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;

namespace RedDog.ServiceBus.Diagnostics
{
    [EventSource(Name = "RedDog-ServiceBus")]
    internal class ServiceBusEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords Queue = (EventKeywords)1;
            public const EventKeywords Topic = (EventKeywords)2;
            public const EventKeywords Subscription = (EventKeywords)4;
            public const EventKeywords Session = (EventKeywords)8;
            public const EventKeywords SessionMessage = (EventKeywords)16;
            public const EventKeywords Receiver = (EventKeywords)32;
            public const EventKeywords Message = (EventKeywords)64;
        }

        public class Tasks
        {
            public const EventTask Send = (EventTask)1;
            public const EventTask Receive = (EventTask)2;
            public const EventTask Received = (EventTask)3;
            public const EventTask Management = (EventTask)4;
            public const EventTask Close = (EventTask)5;
        }

        public static readonly ServiceBusEventSource Log = new ServiceBusEventSource();

        #region Sending
        [Event(1, Message = "Sending to queue: {0}. {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Queue, Task = Tasks.Send)]
        internal void SendToQueue(string ns, string queue, string sessionId, string messageId, string correlationId)
        {
            if (IsEnabled())
                WriteEvent(1, FormatPath(ns, queue), FormatIdentifiers(sessionId, messageId, correlationId), FormatTaskThread());
        }

        [Event(2, Message = "Sending to topic {0}. {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Topic, Task = Tasks.Send)]
        internal void SendToTopic(string ns, string topic, string sessionId, string messageId, string correlationId)
        {
            if (IsEnabled())
                WriteEvent(2, FormatPath(ns, topic), FormatIdentifiers(sessionId, messageId, correlationId), FormatTaskThread());
        }
        #endregion
        #region Management
        [Event(3, Message = "Created Topic: {0}{1}", Level = EventLevel.Informational, Keywords = Keywords.Topic, Task = Tasks.Management)]
        public void CreatedTopic(string ns, string topic)
        {
            if (IsEnabled())
                WriteEvent(3, ns, topic);
        }

        [Event(4, Message = "Created Subscription: {0}{1}/{2}", Level = EventLevel.Informational, Keywords = Keywords.Subscription, Task = Tasks.Management)]
        public void CreatedSubscription(string ns, string topic, string subscription)
        {
            if (IsEnabled())
                WriteEvent(4, ns, topic, subscription);
        }

        [Event(5, Message = "Created Queue: {0}{1}", Level = EventLevel.Informational, Keywords = Keywords.Queue, Task = Tasks.Management)]
        public void CreatedQueue(string ns, string queue)
        {
            if (IsEnabled())
                WriteEvent(5, ns, queue);
        }
        #endregion
        #region Session Receiver
        [Event(6, Message = "Started session receiver: {0}. {1} {2}", Level = EventLevel.Informational, Keywords = Keywords.Session, Task = Tasks.Receive)]
        
        public void StartSessionMessageReceiver(string messageReceiverType, string ns, string path)
        {
            if (IsEnabled())
                WriteEvent(6, messageReceiverType, FormatPath(ns, path), FormatTaskThread());
        }

        [Event(7, Message = "Exception in session receiver for {0}: {1}. {2} {3} {4}", Level = EventLevel.Error, Keywords = Keywords.Session, Task = Tasks.Receive)]
        public void SessionMessageReceiverException(string ns, string path, string sessionId, string messageId, string correlationId, string action, string message, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(7, action, message, FormatIdentifiers(sessionId, messageId, correlationId), FormatPath(ns, path), FormatTaskThread());
        }

        [Event(8, Message = "Session accepted. {0} {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Session, Task = Tasks.Receive)]
        
        public void CreateMessageSessionAsyncHandler(string ns, string path, string sessionId, string messageId, string correlationId)
        {
            if (IsEnabled())
                WriteEvent(8, FormatIdentifiers(sessionId, messageId, correlationId), FormatPath(ns, path), FormatTaskThread());
        }

        [Event(9, Message = "Disposed session handler. {0} {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Session, Task = Tasks.Close)]
        public void DisposeMessageSessionAsyncHandler(string ns, string path, string sessionId)
        {
            if (IsEnabled())
                WriteEvent(9, FormatIdentifiers(sessionId, "", ""), FormatPath(ns, path), FormatTaskThread());
        }

        [Event(10, Message = "Received session message. {0} {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.SessionMessage, Task = Tasks.Received)]
        public void SessionMessageReceived(string ns, string path, string sessionId, string messageId, string correlationId)
        {
            if (IsEnabled())
                WriteEvent(10, FormatIdentifiers(sessionId, messageId, correlationId), FormatPath(ns, path), FormatTaskThread());
        }

        [Event(11, Message = "Closed session. {0} {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Session, Task = Tasks.Close)]
        
        public void SessionClosed(string ns, string path, string sessionId)
        {
            if (IsEnabled())
                WriteEvent(11, FormatIdentifiers(sessionId, "", ""), FormatPath(ns, path), FormatTaskThread());
        }

        [Event(12, Message = "Lost session: {4}. {0} {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Session, Task = Tasks.Close)]
        
        public void SessionLostException(string ns, string path, string sessionId, string message, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(12, FormatIdentifiers(sessionId, "", ""), FormatPath(ns, path), FormatTaskThread(), message);
        }
        #endregion
        #region Message Receiver
        [Event(13, Message = "Started message receiver: {0}. {1} {2}", Level = EventLevel.Informational, Keywords = Keywords.Receiver, Task = Tasks.Receive)]

        public void StartMessageReceiver(string messageReceiverType, string ns, string path)
        {
            if (IsEnabled())
                WriteEvent(13, messageReceiverType, FormatPath(ns, path), FormatTaskThread());
        }

        [Event(14, Message = "Exception in message receiver for {0}: {1}. {2} {3} {4}", Level = EventLevel.Error, Keywords = Keywords.Session, Task = Tasks.Receive)]
        public void MessageReceiverException(string ns, string path, string messageId, string correlationId, string action, string message, string stackTrace)
        {
            if (IsEnabled())
                WriteEvent(14, action, message, FormatIdentifiers(null, messageId, correlationId), FormatPath(ns, path), FormatTaskThread());
        }

        [Event(15, Message = "Received message. {0} {1} {2}", Level = EventLevel.Verbose, Keywords = Keywords.Message, Task = Tasks.Received)]
        public void MessageReceived(string ns, string path, string messageId, string correlationId)
        {
            if (IsEnabled())
                WriteEvent(15, FormatIdentifiers(null, messageId, correlationId), FormatPath(ns, path), FormatTaskThread());
        }
        #endregion

        private string FormatPath(string ns, string path)
        {
            return String.Format("{0}/{1}", ns.Trim('/'), path);
        }

        private string FormatTaskThread()
        {
            return String.Format("[Task:{0} Thread:{1}]", Task.CurrentId ?? 0, Thread.CurrentThread.ManagedThreadId);
        }

        private string FormatIdentifiers(string sessionId, string messageId, string correlationId)
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(sessionId))
                sb.AppendFormat("SessionId: '{0}' ", sessionId);
            if (!String.IsNullOrEmpty(messageId))
                sb.AppendFormat("MessageId: '{0}' ", messageId);
            if (!String.IsNullOrEmpty(correlationId))
                sb.AppendFormat("CorrelationId: '{0}'", correlationId);
            return sb.ToString().Trim();
        }
    }
}