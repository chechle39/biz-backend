using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Models;

namespace CommandsService.Data
{
    public class CarRepo : ICarRepo
    {
        public IEnumerable<Car> GetAllCar()
        {
           var car = new List<Car>();
           return car;
        }
    }
}