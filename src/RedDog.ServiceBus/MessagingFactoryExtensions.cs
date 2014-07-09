using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus
{
    public static class MessagingFactoryExtensions
    {
        internal static string GetShortNamespaceName(this MessagingFactory factory)
        {
            var ns = factory.Address.ToString();
            if (ns.Contains("."))
                ns = ns.Split('.').FirstOrDefault();
            return ns;
        }

        internal async static Task TryCreateEntity(this NamespaceManager ns, Func<NamespaceManager, Task> createDelegate, Func<NamespaceManager, Task> shouldExistDelegate)
        {
            var verifyIfEntityExists = false;

            try
            {
                await createDelegate(ns)
                    .ConfigureAwait(false);
            }
            catch (MessagingEntityAlreadyExistsException)
            {

            }
            catch (MessagingException messagingException)
            {
                if (!messagingException.Message.Contains("SubCode=40901"))
                    throw;
                verifyIfEntityExists = true;
            }

            if (verifyIfEntityExists)
            {
                await shouldExistDelegate(ns)
                    .ConfigureAwait(false);
            }
        }
    }
}
