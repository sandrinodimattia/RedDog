using System.Threading.Tasks;

namespace RedDog.Messenger.Serialization
{
    public interface ISerializer
    {
        Task<byte[]> Serialize<TData>(TData data)
            where TData : class;

        Task<TData> Deserialize<TData>(byte[] serializedMessage)
            where TData : class;
    }
}