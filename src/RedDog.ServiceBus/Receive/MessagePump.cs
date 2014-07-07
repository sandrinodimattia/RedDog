using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus.Receive
{
    public abstract class MessagePump : IMessageReceiver
    {
        private readonly ReceiveMode _mode;

        private readonly string _namespace;

        private readonly string _path;
        
        /// <summary>
        /// Full path to the entity.
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Full namespace of the entity.
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }

        /// <summary>
        /// The receive mode.
        /// </summary>
        public ReceiveMode Mode
        {
            get { return _mode; }
        }

        /// <summary>
        /// Initialize the message pump.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="namespace"></param>
        /// <param name="path"></param>
        protected MessagePump(ReceiveMode mode, string @namespace, string path)
        {
            _mode = mode;
            _namespace = @namespace;
            _path = path;
        }
    }
}