using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Dal.Entities;

namespace CarService.Bll.MakeAppointment
{
    public class WorkerHandler
    {
        public static WorkerUser GetWorkerForTheJob(IList<WorkerUser> workerUsers, DateTime appointment)
        {
            WorkerUser workerForTheJob = new WorkerUser();

            foreach (var worker in workerUsers)
            {
                bool thisWorkerIsFree = true;
                var workingTimes = worker.Works.Select(t => new
                {
                    t.StartingTime,
                    t.EndTime
                }).ToList();

                foreach (var interval in workingTimes)
                {
                    if (appointment >= interval.StartingTime && appointment < interval.EndTime)
                    {
                        thisWorkerIsFree = false;
                    }
                }

                if (thisWorkerIsFree)
                {
                    workerForTheJob = worker;
                    break;
                }
                else
                {
                    throw new Exception("Erre az időpontra, nincs szabad munkatársunk!");
                }
            }

            return workerForTheJob;
        }
    }
}