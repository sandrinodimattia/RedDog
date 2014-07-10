using System;

namespace RedDog.Engine
{
    public interface IJobHost
    {
        event EventHandler Stopped;

        bool IsStopped
        {
            get;
        }
    }
}