using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;
using Microsoft.EntityFrameworkCore;

namespace CarService.Bll.Works
{
    public class ServiceLogic
    {
        private static ApplicationEntityManager _applicationEntityManager;

        public ServiceLogic(CarServiceDbContext context)
        {
            _applicationEntityManager = new ApplicationEntityManager(context);
        }

        public static async Task<IList<Service>> GetMyServices(string id)
        { 
            return await ApplicationEntityManager.GetServcieByUserIdAsync(id);
        }
    }
}
