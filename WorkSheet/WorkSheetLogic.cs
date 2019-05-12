using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Bll.WorkCalendar
{
    public class WorkSheetLogic
    {
        private static ApplicationEntityManager _applicationEntityManager;

        public WorkSheetLogic(CarServiceDbContext context)
        {
            _applicationEntityManager = new ApplicationEntityManager(context);
        }

        public static async Task<IList<Work>> GetRemainingWorksByWorkerIdAsync(string workerId)
        {
            return await ApplicationEntityManager.GetRemainingWorksByWorkerIdAsync(workerId);
        }

        public static Work GetNextWork(IList<Work> works)
        {
            foreach (var work in works)
            {
                if (work.EndTime > DateTime.Now)
                {
                    return work;
                }
            }
            return null;
        }

        public static async Task<Work> GetWorkByIdAsync(int? id)
        {
            return await ApplicationEntityManager.GetWorkByIdAsync(id.Value);
        }

        public static async Task ModifyWorkAsync(Work work)
        {
            await ApplicationEntityManager.ModifyWorkAsync(work);
        }
    }
}
