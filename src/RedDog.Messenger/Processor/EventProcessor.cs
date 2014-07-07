namespace RedDog.Messenger.Processor
{
    public class EventProcessor : MessageProcessor, IEventProcessor
    {
        public EventProcessor(IProcessorConfiguration<IEventProcessorConfiguration> configuration)
            : base(configuration)
        {

        }
    }
}