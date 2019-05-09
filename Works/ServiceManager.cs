using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.Bll.Works
{
    public class ServiceManager
    {
        private static CarServiceDbContext _context;

        public ServiceManager(CarServiceDbContext context)
        {
            _context = context;
        }

        public static async Task<IList<Service>> GetMyServices(string id)
        {
            IList<Service> services = new List<Service>();

            services = await _context.Services
                .Where(w => w.Car.ClientUserId == id)
                .Include(s => s.Car).ToListAsync();

            return services;
        }
    }
}
