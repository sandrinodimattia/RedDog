using System;

using RedDog.Messenger.Bus.Registration;
using RedDog.Messenger.Contracts;

using RedDog.ServiceBus.Send;

namespace RedDog.Messenger.Bus
{
    public interface ICommandBusConfiguration : IBusConfiguration<ICommandBusConfiguration>
    {
        /// <summary>
        /// Create the bus.
        /// </summary>
        /// <returns></returns>
        ICommandBus Build();

        /// <summary>
        /// Register a single command with a sender.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        ICommandBusConfiguration RegisterCommand<TCommand>(IMessageSender sender)
            where TCommand : ICommand;

        /// <summary>
        /// Register one or more commands with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        ICommandBusConfiguration RegisterCommands(IMessageSender sender, Action<IFluentMessageRegistration<ICommand>> config);

        /// <summary>
        /// Register an array of commands with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        ICommandBusConfiguration RegisterCommands(IMessageSender sender, params Type[] types);

        /// <summary>
        /// Register one or more commands with a sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="registrationSource"></param>
        /// <returns></returns>
        ICommandBusConfiguration RegisterCommands(IMessageSender sender, IMessageRegistration<ICommand> registrationSource);
    }
}