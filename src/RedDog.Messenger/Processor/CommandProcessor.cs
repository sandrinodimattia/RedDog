namespace RedDog.Messenger.Processor
{
    public class CommandProcessor : MessageProcessor, ICommandProcessor
    {
        public CommandProcessor(IProcessorConfiguration<ICommandProcessorConfiguration> configuration)
            : base(configuration)
        {

        }
    }
}