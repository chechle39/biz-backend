﻿using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController() 
        { 
        }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("-> inbound Post # Command Service");
            return Ok("Inbound test of from platforms controller");
        }
    }
}
