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

        internal static IDictionary<DayOfWeek, Dictionary<DateTime, bool>> GetFinalOpening(IList<WorkerUser> workerUsers, SubTask subTask)
        {
            IDictionary<DayOfWeek, Dictionary<DateTime, bool>> finalOpening = new Dictionary<DayOfWeek, Dictionary<DateTime, bool>>();
            Dictionary<DayOfWeek, OpeningDay> Openings = new Dictionary<DayOfWeek, OpeningDay>();

            for (int i = 1; i < 8; i++)
            {
                Openings.Add(DateTime.Today.AddDays(i).DayOfWeek, GetOpeningByDayOfWeek(DateTime.Today.AddDays(i).DayOfWeek, subTask.CompanyUser.Opening));
            }
               
            int dayCounter = 0;

            foreach (var o in Openings)
            {
                dayCounter++;
                Dictionary<DateTime, bool> isFreeAtTime = new Dictionary<DateTime, bool>();

                for (DateTime i = o.Value.Start; i < o.Value.End; i = i.AddMinutes(subTask.EstimtedTime))
                {
                    DateTime realDateTime = DateTime.Today;

                    realDateTime = realDateTime.AddDays(dayCounter);
                    realDateTime = realDateTime.AddHours(i.Hour);
                    realDateTime = realDateTime.AddMinutes(i.Minute);

                    bool isWorkerForTheJob = true;

                    if (WorkerHandler.GetWorkerForTheJob(workerUsers, realDateTime, realDateTime.AddMinutes(subTask.EstimtedTime)) == null)
                    {
                        isWorkerForTheJob = false;
                    }                    

                    isFreeAtTime.Add(realDateTime, isWorkerForTheJob);
                }             

                finalOpening.Add(o.Key, isFreeAtTime);                
            }

            return finalOpening;
        }

        private static OpeningDay GetOpeningByDayOfWeek(DayOfWeek dayOfWeek, Opening opening)
        {
            if (dayOfWeek == DayOfWeek.Monday)
            {
                return opening.Monday;
            }
            else if (dayOfWeek == DayOfWeek.Tuesday)
            {
                return opening.Tuesday;
            }
            else if (dayOfWeek == DayOfWeek.Wednesday)
            {
                return opening.Wednesday;
            }
            else if (dayOfWeek == DayOfWeek.Thursday)
            {
                return opening.Thursday;
            }
            else if (dayOfWeek == DayOfWeek.Friday)
            {
                return opening.Friday;
            }
            else if (dayOfWeek == DayOfWeek.Saturday)
            {
                return opening.Saturday;
            }
            else if (dayOfWeek == DayOfWeek.Sunday)
            {
                return opening.Sunday;
            }
            else
            {
                throw new Exception("Nem létező napot adtál meg!");
            }
        }

    }
}
