using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        Task PublishPlatform(PlatformPublishedDto platformPublishedDto);

    }
}
