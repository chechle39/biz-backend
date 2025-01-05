using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsPlatform {platformId}");
            if(!_commandRepo.PlatformsExits(platformId))
            {
                return NotFound();
            }
            var commands = _commandRepo.GetCommandsForPlatform(platformId);
            var map = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(map);
        }

        [HttpGet("{commandId}", Name ="GetCommandForPlatform")]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform {platformId} / {commandId}");
            if (!_commandRepo.PlatformsExits(platformId))
            {
                return NotFound();
            }
            var commands = _commandRepo.GetCommand(platformId, commandId);
            if (commands == null)
            {
                return NotFound();
            }
            var map = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(map);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreatCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreatCommandForPlatform {platformId}");
            if (!_commandRepo.PlatformsExits(platformId))
            {
                return NotFound();
            }
            var command = _mapper.Map<Command>(commandDto);
            _commandRepo.CreateCommad(platformId, command);
            _commandRepo.SaveChange();
            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            var route = CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
            return route;
        }
    }
}
