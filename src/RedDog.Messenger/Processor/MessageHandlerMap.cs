using System;
using System.Collections.Generic;
using System.Linq;

namespace RedDog.Messenger.Processor
{
    public class MessageHandlerMap
    {
        internal Dictionary<Type, List<Type>> HandlerTypes
        {
            get;
            set;
        }

        public MessageHandlerMap()
        {
            HandlerTypes = new Dictionary<Type, List<Type>>();
        }

        public IReadOnlyDictionary<Type, Type[]> GetHandlerTypes()
        {
            return HandlerTypes.ToDictionary(t => t.Key, t => t.Value.ToArray());
        }

        internal void Add(Type messageType, Type handlerType)
        {
            if (!HandlerTypes.ContainsKey(messageType))
            {
                HandlerTypes.Add(messageType, new List<Type>());
            }

            HandlerTypes[messageType].Add(handlerType);
        }
    }
}