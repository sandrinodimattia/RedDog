using RedDog.Messenger.Composition;
using RedDog.Messenger.Filters;
using RedDog.Messenger.Serialization;

namespace RedDog.Messenger.Configuration
{
    public interface IMessagingConfiguration
    {
        IContainer Container
        {
            get;
        }

        ISerializer Serializer
        {
            get;
        }

        MessageFilterInvoker MessageFilterInvoker
        {
            get;
        }
    }
}