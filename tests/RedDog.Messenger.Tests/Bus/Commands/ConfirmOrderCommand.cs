using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Commands
{
    public class ConfirmOrderCommand : ICommand
    {
        public string Id
        {
            get;
            set;
        }
    }
}