﻿using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Bll.Works
{
    public class WorkLogic
    {
        private static ApplicationEntityManager _applicationEntityManager;

        public WorkLogic(CarServiceDbContext context)
        {
            _applicationEntityManager = new ApplicationEntityManager(context);
        }

        public static async Task<IList<Work>> GetWorksByServiceIdAsync(int serviceId)
        {
            return await ApplicationEntityManager.GetWorksByServiceIdAsync(serviceId);
        }

        public static async Task<Work> GetWorkByIdAsync(int id)
        {
            return await ApplicationEntityManager.GetWorkByIdAsync(id);
        }

        public static async Task SaveMessageAsync(Message message)
        {
            await ApplicationEntityManager.SaveMessageAsync(message);
        }
    }
}
