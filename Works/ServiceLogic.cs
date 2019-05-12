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

        public static async Task<IList<Service>> GetMyServicesAsync(string id)
        { 
             return await ApplicationEntityManager.GetServiceByUserIdAsync(id);            
        }

        public static IList<Service> GetMyActiveServices(IList<Service> services)
        {
            List<Service> activeServices = new List<Service>();
            foreach (var service in services)
            {
                foreach (var work in service.Works)
                {
                    if (!work.State.Name.Equals("Finished") && !work.State.Name.Equals("FinishedAndPaid"))
                    {
                        activeServices.Add(service);
                        break;
                    }
                }
            }

            return activeServices;
        }

        public static IList<Service> GetMyFinishedServices(IList<Service> services, IList<Service> activeServices)
        {
            IList<Service> finishedServices = new List<Service>();

            foreach (var service in services)
            {
                bool serviceIsActive = false;
                foreach (var activeService in activeServices)
                {
                    if (service.Id == activeService.Id)
                    {
                        serviceIsActive = true;
                    }
                }

                if (!serviceIsActive)
                {
                    finishedServices.Add(service);
                }
            }

            return finishedServices;
        }

        public static IDictionary<Work, Service> GetFullServiceHistory(IList<Service> services)
        {
            IDictionary<Work, Service> history = new Dictionary<Work, Service>();
            foreach (var service in services)
            {
                foreach (var work in service.Works)
                {
                    
                    history.Add(work, service);
                }
            }
            return history;
        }
    }
}
