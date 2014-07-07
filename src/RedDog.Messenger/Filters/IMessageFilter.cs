using System.Threading.Tasks;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Filters
{
    public interface IMessageFilter
    {
        Task<byte[]> AfterSerialization(IEnvelope envelope, byte[] serializedMessage);

        Task<byte[]> BeforeDeserialization(IEnvelope envelope, byte[] serializedMessage);
    }
}