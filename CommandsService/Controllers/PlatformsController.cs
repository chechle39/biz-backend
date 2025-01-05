using AutoMapper;
using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        public PlatformsController(ICommandRepo commandRepo, IMapper mapper) 
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("-> inbound Post # Command Service");
            return Ok("Inbound test of from platforms controller");
        }
    }
}
