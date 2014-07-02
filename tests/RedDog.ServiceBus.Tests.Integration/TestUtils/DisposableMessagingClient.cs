using System;

using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Tests.Integration.TestUtils
{
    public class DisposableMessagingClient<TClient> : IDisposable
        where TClient : MessageClientEntity
    {
        private readonly MessagingFactory _factory;

        public TClient Client
        {
            get;
            set;
        }

        private bool _disposed;

        public DisposableMessagingClient(MessagingFactory factory, TClient client)
        {
            _factory = factory;
            Client = client;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                OnDispose();
            _disposed = true;
        }

        protected virtual void OnDispose()
        {
            _factory.Close();
            Client.Close();
        }

        ~DisposableMessagingClient()
        {
            Dispose(false);
        }
    }
}