using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Bll.Works
{
    public class WorkManager
    {
        private static CarServiceDbContext _context;

        public WorkManager(CarServiceDbContext context)
        {
            _context = context;
        }

        public static async Task<IList<Work>> GetWorkByServiceAsync(int id)
        {
            IList<Work> works = new List<Work>();

            works = await _context.Works
                .Where(w => w.ServiceId == id)
                .Include(w => w.Service)
                .Include(w => w.State)
                .Include(w => w.SubTask)
                .Include(w => w.WorkerUser)                
                .ToListAsync();

            return works;
        }

    }
}
