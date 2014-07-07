using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public string Id
        {
            get;
            set;
        }
    }
}