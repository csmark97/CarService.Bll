using CarService.Dal.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CarService.Bll.MakeAppointment
{
    public class OpeningHandler
    {
        public static IDictionary<DayOfWeek, OpeningDay> GetOpening(Opening opening)
        {
            Dictionary<DayOfWeek, OpeningDay> Opening = new Dictionary<DayOfWeek, OpeningDay>
                {
                    { DayOfWeek.Monday, opening.Monday },
                    { DayOfWeek.Tuesday, opening.Tuesday },
                    { DayOfWeek.Wednesday, opening.Wednesday },
                    { DayOfWeek.Thursday, opening.Thursday },
                    { DayOfWeek.Friday, opening.Friday },
                    { DayOfWeek.Saturday, opening.Saturday },
                    { DayOfWeek.Sunday, opening.Sunday }
                };
            
            return Opening;
        }

        internal static IDictionary<DayOfWeek, Dictionary<DateTime, bool>> GetOpening2(IList<WorkerUser> workerUsers, Opening opening)
        {
            IDictionary<DayOfWeek, Dictionary<DateTime, bool>> finalOpening = new Dictionary<DayOfWeek, Dictionary<DateTime, bool>>();
            Dictionary<DayOfWeek, OpeningDay> Openings = new Dictionary<DayOfWeek, OpeningDay>
                {
                    { DayOfWeek.Monday, opening.Monday },
                    { DayOfWeek.Tuesday, opening.Tuesday },
                    { DayOfWeek.Wednesday, opening.Wednesday },
                    { DayOfWeek.Thursday, opening.Thursday },
                    { DayOfWeek.Friday, opening.Friday },
                    { DayOfWeek.Saturday, opening.Saturday },
                    { DayOfWeek.Sunday, opening.Sunday }
                };
            //TODO
            foreach (var o in Openings)
            {   
                //for(int i = o.Value.Start.Hour; i < o.Value.End.Hour)
                //{

                //}
                bool isWorkerForTheJob = true;
                if (WorkerHandler.GetWorkerForTheJob(workerUsers, o.Value.Start) == null)
                {
                    isWorkerForTheJob = false;
                }
                Dictionary<DateTime, bool> isFreeAtTime = new Dictionary<DateTime, bool>();

                DateTime realDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, o.Value.Start.Hour, o.Value.Start.Minute, 0);

                isFreeAtTime.Add(realDateTime, isWorkerForTheJob);

                finalOpening.Add(o.Key, isFreeAtTime);
            }

            return finalOpening;
        }
    }
}
