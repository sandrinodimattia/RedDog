using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using RedDog.Messenger.Contracts;

namespace RedDog.Messenger.Filters.Serialization
{
    public class GzipCompressionMessageFilter : IMessageFilter
    {
        public async Task<byte[]> AfterSerialization(IEnvelope envelope, byte[] serializedMessage)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(serializedMessage))
                using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
                    await inputStream.CopyToAsync(compressionStream).ConfigureAwait(false);
                envelope.Properties[MessageProperties.Compression] = "Gzip";
                return outputStream.ToArray();
            }
        }

        public async Task<byte[]> BeforeDeserialization(IEnvelope envelope, byte[] serializedMessage)
        {
            using (var inputStream = new MemoryStream(serializedMessage))
            using (var decompressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                await decompressionStream.CopyToAsync(outputStream).ConfigureAwait(false);
                return outputStream.ToArray();
            }
        }
    }
}