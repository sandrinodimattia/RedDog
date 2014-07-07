using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedDog.Messenger.Serialization.Json
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        private readonly bool _indented;

        private readonly JsonSerializerSettings _settings;

        public NewtonsoftJsonSerializer(bool indented = false)
            : this(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple }, indented)
        {

        }

        public NewtonsoftJsonSerializer(JsonSerializerSettings settings, bool indented = false)
        {
            _settings = settings;
            _indented = indented;
        }

        public Task<byte[]> Serialize<TData>(TData data)
            where TData : class
        {
            try
            {
                return Task.FromResult(
                    Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(data, _indented ? Formatting.Indented : Formatting.None, _settings)));
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializationException(ex.Message, ex);
            }
        }

        public Task<TData> Deserialize<TData>(byte[] serializedMessage)
            where TData : class
        {
            try
            {
                return Task.FromResult(
                    JsonConvert.DeserializeObject(
                        Encoding.UTF8.GetString(serializedMessage), _settings) as TData);
            }
            catch (JsonSerializationException e)
            {
                throw new SerializationException(e.Message, e);
            }
        }
    }
}