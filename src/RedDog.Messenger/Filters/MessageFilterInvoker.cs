using System.Collections.Generic;
using System.Threading.Tasks;

using RedDog.Messenger.Contracts;
using RedDog.Messenger.Diagnostics;

namespace RedDog.Messenger.Filters
{
    public class MessageFilterInvoker
    {
        private readonly List<IMessageFilter> _filters;

        public MessageFilterInvoker()
        {
            _filters = new List<IMessageFilter>();
        }

        public void Add(IMessageFilter filter)
        {
            _filters.Add(filter);
        }

        public async Task<byte[]> AfterSerialization(IEnvelope envelope, byte[] serializedMessage)
        {
            foreach (var filter in _filters)
            {
                MessagingEventSource.Log.AfterSerialization(filter, envelope);

                // Intercept.
                serializedMessage = await filter.AfterSerialization(envelope, serializedMessage).ConfigureAwait(false);
            }

            return serializedMessage;
        }

        public async Task<byte[]> BeforeDeserialization(IEnvelope envelope, byte[] serializedMessage)
        {
            foreach (var interceptor in _filters)
            {
                MessagingEventSource.Log.BeforeDeserialization(interceptor.GetType().Name, envelope.MessageId, envelope.CorrelationId, envelope.SessionId);

                // Intercept.
                serializedMessage = await interceptor.BeforeDeserialization(envelope, serializedMessage).ConfigureAwait(false);
            }

            return serializedMessage;
        }
    }
}