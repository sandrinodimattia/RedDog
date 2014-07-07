using System;
using System.Linq;
using System.Reflection;

using RedDog.Messenger.Contracts;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public static class CommandBusConfigurationExtensions
    {
        public static ICommandBusConfiguration RegisterCommands(this ICommandBusConfiguration configuration, IMessageSender sender, Assembly assembly, Func<IQueryable<Type>, IQueryable<Type>> typeFilter = null)
        {
            // Get types.
            IQueryable<Type> types = assembly.GetTypes()
                .AsQueryable()
                .Where(t => typeof(ICommand).IsAssignableFrom(t));
            if (typeFilter != null)
                types = typeFilter(types);

            // Add types.
            configuration.RegisterCommands(sender, types.ToArray());

            // Done.
            return configuration;
        }
    }
}