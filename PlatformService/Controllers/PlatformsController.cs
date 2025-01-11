using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        public PlatformsController(
            IPlatformRepo platformRepo,
            ICommandDataClient commandDataClient ,
            IMapper mapper,
            IMessageBusClient messageBusClient)
        {
            _platformRepo = platformRepo;
            _commandDataClient= commandDataClient;
            _mapper = mapper;
            _messageBusClient= messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("->> Getting platforms...");
            var platformItem = _platformRepo.GetAllPlatform();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id) 
        {
            Console.WriteLine("->> Getting platforms by id...");
            var platformItem = _platformRepo.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform)
        {
            var map = _mapper.Map<Platform>(platform);
            _platformRepo.CreatePlatform(map);
            _platformRepo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(map);
            // Send sync message
            try
            {
                Console.WriteLine("Call SendPlatformToCommand");
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Call SendPlatformToCommand {ex.Message}");
            }
            // Send async message
            try
            {
                var platformpublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformpublishedDto.Event = "Platform_published";
                await _messageBusClient.PublishPlatform(platformpublishedDto);
            }
            catch (Exception e) 
            { 
            }
            var route = CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
            return route;
        }
    }
}
