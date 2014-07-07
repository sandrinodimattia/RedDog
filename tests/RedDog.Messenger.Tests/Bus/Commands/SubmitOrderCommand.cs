using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Tests.Bus.Commands
{
    public class SubmitOrderCommand : ICommand
    {
        public string Id
        {
            get;
            set;
        }
    }
}