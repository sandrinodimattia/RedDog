using RedDog.Messenger.Composition;
using RedDog.Messenger.Diagnostics;
using RedDog.Messenger.Filters;
using RedDog.Messenger.Serialization;

namespace RedDog.Messenger.Configuration
{
    public class MessagingConfiguration<TConfiguration> : IMessagingConfiguration, IMessagingContainerConfiguration<TConfiguration>, IMessagingSerializerConfiguration<TConfiguration>
        where TConfiguration : class, IMessagingConfiguration
    {
        public MessagingConfiguration()
        {
            MessageFilterInvoker = new MessageFilterInvoker();
        }

        public IContainer Container
        {
            get;
            private set;
        }

        public ISerializer Serializer
        {
            get;
            private set;
        }

        public MessageFilterInvoker MessageFilterInvoker
        {
            get;
            private set;
        }

        public TConfiguration WithContainer(IContainer container)
        {
            Container = container;

            // Log.
            MessengerEventSource.Log.RegisteredContainer(GetType().Name, container);

            // Continue.
            return this as TConfiguration;
        }

        public TConfiguration WithSerializer(ISerializer serializer)
        {
            Serializer = serializer;

            // Log.
            MessengerEventSource.Log.RegisteredSerializer(GetType().Name, serializer);

            // Continue.
            return this as TConfiguration;
        }

        public TConfiguration WithSerializationFilter(IMessageFilter filter)
        {
            MessageFilterInvoker.Add(filter);

            // Log.
            MessengerEventSource.Log.RegisteredSerializationFilter(GetType().Name, filter);

            // Continue.
            return this as TConfiguration;
        }
    }
}