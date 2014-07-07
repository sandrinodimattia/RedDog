using System;

using Autofac;

namespace RedDog.Messenger.Containers.Autofac
{
    public class AutofacContainer : Composition.IContainer
    {
        private readonly ILifetimeScope _scope;

        public AutofacContainer(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public Composition.IContainer BeginScope()
        {
            return new AutofacContainer(_scope.BeginLifetimeScope());
        }

        public T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }
    }
}