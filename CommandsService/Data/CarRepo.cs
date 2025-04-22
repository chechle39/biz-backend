using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Models;

namespace CommandsService.Data
{
    public class CarRepo : ICarRepo
    {
        
        private readonly AppDbContext _context;
        public CarRepo(AppDbContext context) 
        {
            _context = context;
        }
        public void CreateCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }
            _context.cars.Add(car);
        }

        public IEnumerable<Car> GetAllCar()
        {
           var car = new List<Car>();
           return car;
        }
        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}