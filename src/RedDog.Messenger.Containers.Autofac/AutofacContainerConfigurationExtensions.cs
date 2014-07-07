using Autofac;

using RedDog.Messenger.Configuration;

namespace RedDog.Messenger.Containers.Autofac
{
    public static class AutofacContainerConfigurationExtensions
    {
        public static TConfiguration WithAutofac<TConfiguration>(this TConfiguration configuration, IComponentContext context)
            where TConfiguration : IMessagingConfiguration, IMessagingContainerConfiguration<TConfiguration>
        {
            return configuration.WithContainer(new AutofacContainer(context.Resolve<ILifetimeScope>()));
        }

        public static TConfiguration WithAutofac<TConfiguration>(this TConfiguration configuration, ILifetimeScope scope)
            where TConfiguration : IMessagingConfiguration, IMessagingContainerConfiguration<TConfiguration>
        {
            return configuration.WithContainer(new AutofacContainer(scope));
        }
    }
}
