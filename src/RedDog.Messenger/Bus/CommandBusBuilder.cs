using System;

using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public class CommandBusBuilder : BusBuilder<ICommandBusConfiguration>, ICommandBusConfiguration
    {
        /// <summary>
        /// Build the bus.
        /// </summary>
        /// <returns></returns>
        public ICommandBus Build()
        {
            return new CommandBus(this);
        }

        /// <summary>
        /// Register a single command with a sender.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ICommandBusConfiguration RegisterCommand<TCommand>(IMessageSender sender)
            where TCommand : ICommand
        {
            return RegisterCommands(sender, new TypeMessageRegistration<ICommand>(typeof(TCommand)));
        }

        /// <summary>
        /// Register one or more commands with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public ICommandBusConfiguration RegisterCommands(IMessageSender sender, Action<IFluentMessageRegistration<ICommand>> config)
        {
            // Start the fluent configuration.
            var registration = new FluentMessageRegistration<ICommand>();
            config(registration);

            // Register.
            return RegisterCommands(sender, registration);
        }

        /// <summary>
        /// Register an array of commands with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public ICommandBusConfiguration RegisterCommands(IMessageSender sender, params Type[] types)
        {
            return RegisterCommands(sender, new ArrayMessageRegistration<ICommand>(types));
        }

        /// <summary>
        /// Register one or more commands with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        public ICommandBusConfiguration RegisterCommands(IMessageSender sender, IMessageRegistration<ICommand> registrationSource)
        {
            foreach (var type in registrationSource.GetTypes())
            {
                AddMessageType(type, sender);
            }

            return this;
        }

        /// <summary>
        /// Create the CommandBus builder.
        /// </summary>
        /// <returns></returns>
        public static ICommandBusConfiguration Create()
        {
            return new CommandBusBuilder();
        }
    }
}