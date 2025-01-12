using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void processEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.platformPublished:
                    // todo
                    Addplatform(message);
                    break;
                default:
                    break;
            }

        }

        private EventType DetermineEvent(string nitificationMessage)
        {
            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(nitificationMessage);
            switch (eventType.Event)
            {
                case "Platform_published":
                    Console.WriteLine("Platform published event detected");
                    return EventType.platformPublished;
                default:

                    Console.WriteLine("Could not determine  the event type");
                    return EventType.platformPublished;
            }
        }

        private void Addplatform(string platformPublisheMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformpublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublisheMessage);
                try
                {
                    var plat = _mapper.Map<Platform>(platformpublishedDto);
                    if (!repo.ExternalPlatformsExits(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChange();
                        Console.WriteLine($"--> Platform added...");
                    }
                    else
                    {
                        Console.WriteLine($"--> Platform already exits");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not add platform to DB {ex.Message}");
                }

            }
        }

    }
    enum EventType
    {
        platformPublished,
        Undetermined,
    }
}
