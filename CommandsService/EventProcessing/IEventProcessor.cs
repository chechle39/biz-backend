namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void processEvent(string message);
    }
}
