using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICarRepo
    {
         IEnumerable<Car> GetAllCar();
    }
}