using System;
using System.Linq;
using System.Reflection;

using RedDog.Messenger.Contracts;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public static class EventBusConfigurationExtensions
    {
        public static IEventBusConfiguration RegisterEvents(this IEventBusConfiguration configuration, IMessageSender sender, Assembly assembly, Func<IQueryable<Type>, IQueryable<Type>> typeFilter = null)
        {
            // Get types.
            IQueryable<Type> types = assembly.GetTypes()
                .AsQueryable()
                .Where(t => typeof(IEvent).IsAssignableFrom(t));
            if (typeFilter != null)
                types = typeFilter(types);

            // Add types.
            configuration.RegisterEvents(sender, types.ToArray());

            // Done.
            return configuration;
        }
    }
}