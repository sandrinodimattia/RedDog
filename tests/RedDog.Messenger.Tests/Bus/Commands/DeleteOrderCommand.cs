using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Commands
{
    public class DeleteOrderCommand : ICommand
    {
        public string Id
        {
            get;
            set;
        }
    }
}
