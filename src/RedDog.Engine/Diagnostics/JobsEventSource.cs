using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;

namespace RedDog.Engine.Diagnostics
{
    [EventSource(Name = "RedDog-Jobs")]
    public class JobsEventSource : EventSource
    {
        public static readonly JobsEventSource Log = new JobsEventSource();

        public class Tasks
        {
            public const EventTask Initialize = (EventTask)1;
            public const EventTask Start = (EventTask)2;
            public const EventTask Run = (EventTask)3;
            public const EventTask Stop = (EventTask)4;
        }

        [Event(1, Message = "Initializing job host with '{0} jobs'.", Level = EventLevel.Informational, Task = Tasks.Initialize)]
        internal void JobHostInitializing(int jobCount)
        {
            if (IsEnabled())
            {
                WriteEvent(1, jobCount);
            }
        }

        [Event(2, Message = "Initializing job '{0}'.", Level = EventLevel.Informational, Task = Tasks.Initialize)]
        internal void JobInitializing(string jobName)
        {
            if (IsEnabled())
            {
                WriteEvent(2, jobName);
            }
        }

        [Event(3, Message = "Job '{0}' initialized.", Level = EventLevel.Informational, Task = Tasks.Initialize)]
        internal void JobInitialized(string jobName)
        {
            if (IsEnabled())
            {
                WriteEvent(3, jobName);
            }
        }

        [Event(4, Message = "'{1}' while initializing job '{0}': {2}.", Level = EventLevel.Critical, Task = Tasks.Initialize)]
        internal void JobInitializationError(string name, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
            {
                WriteEvent(4, name, exceptionType, exceptionMessage, stackTrace);
            }
        }

        [Event(5, Message = "Starting '{0} jobs'.", Level = EventLevel.Informational, Task = Tasks.Start)]
        internal void JobHostStarting(int jobCount)
        {
            if (IsEnabled())
            {
                WriteEvent(5, jobCount);
            }
        }

        [Event(6, Message = "Scheduling '{0} jobs'.", Level = EventLevel.Informational, Task = Tasks.Start)]
        internal void JobsScheduling(int jobCount)
        {
            if (IsEnabled())
            {
                WriteEvent(6, jobCount);
            }
        }

        [Event(7, Message = "Scheduling job '{0}' to start at '{2}' and run every '{1}'.", Level = EventLevel.Informational, Task = Tasks.Start)]
        internal void JobScheduling(string jobName, string runEvery, string startedAt)
        {
            if (IsEnabled())
            {
                WriteEvent(7, jobName, runEvery, startedAt);
            }
        }

        [Event(8, Message = "'{0}' while scheduling jobs: {1}.", Level = EventLevel.Critical, Task = Tasks.Start)]
        internal void JobsSchedulingError(string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
            {
                WriteEvent(8, exceptionType, exceptionMessage, stackTrace);
            }
        }

        [Event(9, Message = "All jobs have been scheduled.", Level = EventLevel.Informational, Task = Tasks.Start)]
        internal void JobsScheduled()
        {
            if (IsEnabled())
            {
                WriteEvent(9);
            }
        }

        [Event(10, Message = "Shutdown signal received. Shutting down jobs.", Level = EventLevel.Informational, Task = Tasks.Stop)]
        internal void JobsShuttingDown()
        {
            if (IsEnabled())
            {
                WriteEvent(10);
            }
        }

        [Event(11, Message = "'{0}' while shutting down jobs: {1}", Level = EventLevel.Error, Task = Tasks.Stop)]
        internal void JobsShutDownError(string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
            {
                WriteEvent(11, exceptionType, exceptionMessage, stackTrace);
            }
        }

        [Event(12, Message = "All jobs have been shut down.", Level = EventLevel.Informational, Task = Tasks.Stop)]
        internal void JobsShutDown()
        {
            if (IsEnabled())
            {
                WriteEvent(12);
            }
        }
        
        [Event(50, Message = "Executing job: '{0}'.", Level = EventLevel.Verbose, Task = Tasks.Run)]
        internal void JobExecuting(string jobName)
        {
            if (IsEnabled())
            {
                WriteEvent(50, jobName);
            }
        }

        [Event(51, Message = "'{1}' while executing job '{0}': {2}.", Level = EventLevel.Error, Task = Tasks.Run)]
        internal void JobExecutionError(string jobName, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
            {
                WriteEvent(51, jobName, exceptionType, exceptionMessage, stackTrace);
            }
        }

        [NonEvent]
        internal void TaskExecuting(string taskName)
        {
            TaskExecuting(taskName, Task.CurrentId ?? 0, Thread.CurrentThread.ManagedThreadId);
        }

        [Event(70, Message = "Executing task: '{0}' (Task: {1} / Thread: {2}).", Level = EventLevel.Verbose, Task = Tasks.Run)]
        internal void TaskExecuting(string taskName, int taskId, int threadId)
        {
            if (IsEnabled())
            {
                WriteEvent(70, taskName, taskId, threadId);
            }
        }

        [Event(71, Message = "'{1}' executing task '{0}': {2}.", Level = EventLevel.Error, Task = Tasks.Run)]
        internal void TaskExecutionError(string taskName, string exceptionType, string exceptionMessage, string stackTrace)
        {
            if (IsEnabled())
            {
                WriteEvent(71, taskName, exceptionType, exceptionMessage, stackTrace);
            }
        }

        [Event(72, Message = "Executed task '{0}' in '{1}'.", Level = EventLevel.Verbose, Task = Tasks.Run)]
        internal void TaskExecuted(string taskName, string duration)
        {
            if (IsEnabled())
            {
                WriteEvent(72, taskName, duration);
            }
        }
    }
}
