using System.Collections.Generic;

namespace RedDog.Messenger.Contracts
{
    public interface IMetadataProvider
    {
        IDictionary<string, object> Properties
        {
            get;
        }
    }
}