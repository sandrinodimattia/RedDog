using Microsoft.ServiceBus.Messaging;

namespace RedDog.ServiceBus
{
    public static class BrokeredMessageExtensions
    {
        public static bool TryComplete(this BrokeredMessage msg)
        {
            try
            {
                // Mark brokered message as complete.
                msg.Complete();

                // Return a result indicating that the message has been completed successfully.
                return true;
            }
            catch (MessageLockLostException)
            {
                // It's too late to compensate the loss of a message lock. We should just ignore it so that it does not break the receive loop.
                // We should be prepared to receive the same message again.
            }
            catch (MessagingException)
            {
                // There is nothing we can do as the connection may have been lost, 
                // or the underlying topic/subscription may have been removed.
                // If Complete() fails with this exception, the only recourse is to prepare to receive another message (possibly the same one).
            }

            return false;
        }

        public static bool TryAbandon(this BrokeredMessage msg)
        {
            try
            {
                // Abandons a brokered message. This will cause the Service Bus to
                // unlock the message and make it available to be received again, 
                // either by the same consumer or by another competing consumer.
                msg.Abandon();

                // Return a result indicating that the message has been abandoned successfully.
                return true;
            }
            catch (MessageLockLostException)
            {
                // It's too late to compensate the loss of a message lock.
                // We should just ignore it so that it does not break the receive loop.
                // We should be prepared to receive the same message again.
            }
            catch (MessagingException)
            {
                // There is nothing we can do as the connection may have been lost,
                //  or the underlying topic/subscription may have been removed.
                // If Abandon() fails with this exception, the only recourse is to receive another message (possibly the same one).
            }

            return false;
        }
    }
}
