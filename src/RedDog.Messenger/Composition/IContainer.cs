using System;

namespace RedDog.Messenger.Composition
{
    public interface IContainer : IDisposable
    {
        IContainer BeginScope();

        T Resolve<T>();

        object Resolve(Type type);
    }
}