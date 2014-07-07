using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Commands
{
    public class CancelOrderCommand : ICommand
    {
        public string Id
        {
            get;
            set;
        }
    }
}