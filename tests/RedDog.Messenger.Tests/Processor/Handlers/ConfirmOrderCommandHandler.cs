using System.Threading.Tasks;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Commands;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand>
    {
        public Task Handle(ConfirmOrderCommand command)
        {
            return Task.FromResult(false);
        }
    }
}
