using System;
using RedDog.Engine.Diagnostics;

namespace RedDog.Engine
{
    public abstract class Job
    {
        public virtual string Name
        {
            get { return GetType().Name; }
        }

        public virtual TimeSpan Interval
        {
            get { return TimeSpan.FromDays(1); }
        }


        public virtual TimeSpan StartOffset
        {
            get { return TimeSpan.Zero; }
        }
        
        public virtual void Initialize()
        {

        }

        public void Execute(ITask task, bool throwOnError = false)
        {
            JobsEventSource.Log.TaskExecuting(task.GetType().Name);

            var startTime = DateTime.UtcNow;

            try
            {
                task.Execute();

                JobsEventSource.Log.TaskExecuted(task.GetType().Name, (DateTime.UtcNow - startTime).ToString());
            }
            catch (Exception ex)
            {
                JobsEventSource.Log.TaskExecutionError(task.GetType().Name, ex.GetType().Name, ex.Message, ex.StackTrace);

                // Make sure the exception bubbles up and stops the execution of other tasks.
                if (throwOnError)
                    throw;
            }
        }

        public abstract void RunOnce();
    }
}