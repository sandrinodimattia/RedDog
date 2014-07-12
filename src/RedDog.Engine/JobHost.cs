using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Collections.Generic;

using RedDog.Engine.Diagnostics;

namespace RedDog.Engine
{
    public class JobHost : IDisposable, IJobHost
    {
        private readonly AsyncSubject<Unit> _shutdownSignal;

        public event EventHandler Stopped;

        public bool IsStopped
        {
            get;
            private set;
        }

        public IDictionary<string, Job> Jobs 
        { 
            get; 
            private set; 
        }

        public JobHost()
        {
            // Job container.
            Jobs = new Dictionary<string, Job>();

            // Signal 
            _shutdownSignal = new AsyncSubject<Unit>();
            _shutdownSignal.OnNext(Unit.Default);
        }

        /// <summary>
        /// Add a job to the runner.
        /// </summary>
        /// <param name="job"></param>
        public void Add(Job job)
        {
            Jobs.Add(job.Name, job);
        }

        /// <summary>
        /// Initialize every job.
        /// </summary>
        public void Initialize()
        {
            InitializeJobs(Jobs.Values.ToArray());
        }

        /// <summary>
        /// Initialize every job.
        /// </summary>
        private void InitializeJobs(Job[] jobs)
        {
            JobsEventSource.Log.JobHostInitializing(jobs.Count());

            // Initialize every job.
            foreach (var job in jobs)
            {
                try
                {
                    JobsEventSource.Log.JobInitializing(job.Name);

                    job.Initialize();

                    JobsEventSource.Log.JobInitialized(job.Name);
                }
                catch (Exception ex)
                {
                    JobsEventSource.Log.JobInitializationError(job.Name, ex.GetType().Name, ex.Message, ex.StackTrace);
                    throw;
                }
            }
        }

        /// <summary>
        /// Start every job.
        /// </summary>
        public void Start()
        {
            JobsEventSource.Log.JobHostStarting(Jobs.Count);

            // Schedule.
            ScheduleJobs(Jobs);
        }
        
        /// <summary>
        /// Schedule every job.
        /// </summary>
        /// <param name="jobs"></param>
        private void ScheduleJobs(IDictionary<string, Job> jobs)
        {
            JobsEventSource.Log.JobsScheduling(jobs.Count);

            // Create schedules.
            IDisposable[] subscriptions;
            try
            {
                subscriptions = jobs.Select(job => ScheduleJob(job.Key, job.Value)).ToArray();
            }
            catch (Exception ex)
            {
                JobsEventSource.Log.JobsSchedulingError(ex.GetType().Name, ex.Message, ex.StackTrace);
                throw;
            }

            JobsEventSource.Log.JobsScheduled();

            // Wait for stop.
            _shutdownSignal.Wait();

            JobsEventSource.Log.JobsShuttingDown();

            // Stop every timer.
            foreach (var token in subscriptions)
            {
                token.Dispose();
            }
        }

        /// <summary>
        /// Schedule a single job.
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        private IDisposable ScheduleJob(string jobType, Job job)
        {
            var startTime = DateTimeOffset.UtcNow + job.StartOffset;

            // Log.
            JobsEventSource.Log.JobScheduling(jobType, job.Interval.ToString(), startTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));

            // Create an infinite loop.
            return Observable.Timer(startTime, job.Interval).Subscribe(_ => RunJob(jobType, job));
        }

        /// <summary>
        /// Run a single job.
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="job"></param>
        private void RunJob(string jobName, Job job)
        {
            JobsEventSource.Log.JobExecuting(jobName);

            try
            {
                job.RunOnce();
            }
            catch (Exception ex)
            {
                JobsEventSource.Log.JobExecutionError(jobName, ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Stop the job runner.
        /// </summary>
        public void Stop()
        {
            _shutdownSignal.OnCompleted();

            IsStopped = true;

            try
            {
                if (Stopped != null)
                    Stopped(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                JobsEventSource.Log.JobsShutDownError(ex.GetType().Name, ex.Message, ex.StackTrace);
            }

            JobsEventSource.Log.JobsShutDown();
        }

        public void Dispose()
        {
            _shutdownSignal.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
