using CarService.Dal.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CarService.Bll.Client.MakeAppointment
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
    }
}
