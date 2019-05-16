using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Bll.Company
{
    public class CompanyLogic
    {
        private readonly ApplicationEntityManager _applicationEntityManager;

        public CompanyLogic(CarServiceDbContext carServiceDbContext)
        {
            _applicationEntityManager = new ApplicationEntityManager(carServiceDbContext);
        }

        public static async Task<IList<Service>> GetServicesByCompanyIdAsync(CompanyUser user)
        {
            return await ApplicationEntityManager.GetServicesByCompanyIdAsync(user);
        }

        public static async Task<Service> GetServiceByIdAsync(int id)
        {
            return await ApplicationEntityManager.GetServiceByIdAsync(id);
        }
    }
}
