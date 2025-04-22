using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        // private readonly ILogger<CarController> _logger;

        private readonly ICarRepo _carRepo;
        private readonly IMapper _mapper;
        public CarController(ICarRepo carRepo, IMapper mapper)
        {
            _carRepo = carRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Car>> GetCommandsPlatform(int platformId)
        {
            var car = _carRepo.GetAllCar();
            return Ok(car);
        }
    }
}