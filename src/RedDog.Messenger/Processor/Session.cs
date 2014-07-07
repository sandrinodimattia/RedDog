using System.IO;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

using RedDog.Messenger.Contracts;
using RedDog.Messenger.Serialization;

namespace RedDog.Messenger.Processor
{
    internal class Session : ISession
    {
        private readonly MessageSession _session;

        private readonly ISerializer _serializer;

        public Session(MessageSession session, ISerializer serializer)
        {
            _session = session;
            _serializer = serializer;
        }

        public string Id
        {
            get { return _session.SessionId; }
        }

        /// <summary>
        /// Read the state.
        /// </summary>
        /// <returns></returns>
        private async Task<byte[]> ReadState()
        {
            using (var stream = new MemoryStream())
            {
                var stateStream = await _session.GetStateAsync();
                stateStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Read the session state.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        public async Task<TState> GetState<TState>()
            where TState : class 
        {
            return await _serializer.Deserialize<TState>(await ReadState());
        }

        /// <summary>
        /// Persiste the session state.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task SetState<TState>(TState state)
            where TState : class 
        {
            using (var stream = new MemoryStream(await _serializer.Serialize(state)))
            {
                stream.Position = 0;
                await _session.SetStateAsync(stream);
            }
        } 
    }
}