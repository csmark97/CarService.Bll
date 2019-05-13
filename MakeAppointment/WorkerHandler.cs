using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;

namespace CarService.Bll.MakeAppointment
{
    public class WorkerHandler
    {
        public static WorkerUser GetWorkerForTheJob(IList<WorkerUser> workerUsers, DateTime appointmentStart, DateTime appointmentEnd)
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
                    if (appointmentStart >= interval.StartingTime && appointmentStart < interval.EndTime
                            || appointmentEnd > interval.StartingTime && appointmentEnd <= interval.EndTime)
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
                    return null;
                }
            }

            return workerForTheJob;
        }       
    }
}