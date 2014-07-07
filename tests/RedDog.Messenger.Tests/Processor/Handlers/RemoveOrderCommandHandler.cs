using System.Threading.Tasks;
using RedDog.Messenger.Contracts.Handlers;
using RedDog.Messenger.Tests.Bus.Commands;

namespace RedDog.Messenger.Tests.Processor.Handlers
{
    public class RemoveOrderCommandHandler : ICommandHandler<CancelOrderCommand>, ICommandHandler<DeleteOrderCommand>
    {
        public Task Handle(CancelOrderCommand command)
        {
            return Task.FromResult(false);
        }

        public Task Handle(DeleteOrderCommand command)
        {
            return Task.FromResult(false);
        }
    }
}
