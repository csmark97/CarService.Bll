using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Web.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Bll.Client.MakeAppointment
{
    public class AppointmentManager
    {
        private static CarServiceDbContext _context;

        public AppointmentManager(CarServiceDbContext context)
        {
            _context = context;
        }

        public static async Task<SubTask> GetSubTaskAsync(int id)
        {
            return await _context.SubTasks.FirstOrDefaultAsync(s => s.Id == id);
        }

        public static IDictionary<DayOfWeek, OpeningDay> GetOpening(Opening opening)
        {
            return OpeningHandler.GetOpening(opening);
        }

        public static IList<SelectListItem> GetCarsAsync(ClientUser clientUser)
        {
            throw new NotImplementedException();
        }

        public static async Task<IList<SelectListItem>> GetCarsAsync(string userId)
        {
            IList<Car> cars = await _context.Cars.Where(w => w.ClientUserId == userId).ToListAsync();

            IList<SelectListItem> selectListOfCars = new List<SelectListItem>();
            foreach (var car in cars)
            {
                SelectListItem carItem = new SelectListItem
                {
                    Value = car.Id.ToString(),
                    Text = car.Model + " " + car.YearOfManufacture
                };
                selectListOfCars.Add(carItem);
            }

            return selectListOfCars;
        }

        public static DateTime CreateAppointmentDate(string[] elementsOfDate, string[] elementsOfTime)
        {
            int year, month, day;
            year = int.Parse(elementsOfDate[0]);
            month = int.Parse(elementsOfDate[1]);
            day = int.Parse(elementsOfDate[2]);

            int hour, minute;
            hour = int.Parse(elementsOfTime[0]);
            minute = int.Parse(elementsOfTime[1]);

            DateTime appointment = new DateTime(year, month, day, hour, minute, 0);

            return appointment;
        }

        private static int? ServiceExists(int carId)
        {
            bool carHasService = _context.Services.Any(e => e.Id == carId);

            if (carHasService)
            {
                var services = _context.Services
                    .Where(e => e.Id == carId)
                    .Select(w => new
                    {
                        w.Id,
                        works = w.Works
                    })
                    .ToList();

                foreach (var service in services)
                {
                    foreach (var work in service.works)
                    {
                        if (!work.State.Equals("Finished"))
                        {
                            return service.Id;
                        }
                    }
                }
            }

            return null;
        }

        public static async Task MakeAppointmentAsync(DateTime appointment, int carId, SubTask subTask)
        {
            IList<WorkerUser> workerUsers = await _context.WorkerUsers.ToArrayAsync();

            workerUsers.Shuffle();

            WorkerUser workerForTheJob = WorkerHandler.GetWorkerForTheJob(workerUsers, appointment);           

            int? openServiceId = ServiceExists(carId);

            Service service;
            if (!openServiceId.HasValue)
            {
                service = new Service
                {
                    StartingTime = appointment,
                    EndTime = appointment.AddMinutes(subTask.EstimtedTime),
                    TotalPrice = subTask.EstimatedPrice,
                    CarId = carId
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();
            }
            else
            {
                service = await _context.Services.Where(w => w.Id == openServiceId.Value).FirstOrDefaultAsync();
                service.TotalPrice += subTask.EstimatedPrice;

                if (appointment.AddMinutes(subTask.EstimtedTime) > service.EndTime)
                {
                    service.EndTime = appointment.AddMinutes(subTask.EstimtedTime);
                }

                _context.Services.Add(service);
                _context.Attach(service).State = EntityState.Modified;
            }

            Work work = new Work
            {
                StartingTime = appointment,
                EndTime = appointment.AddMinutes(subTask.EstimtedTime),
                Price = subTask.EstimatedPrice,
                SubTaskId = subTask.Id,
                ServiceId = service.Id,
                StateId = 2,
                WorkerUserId = workerForTheJob.Id
            };

            _context.Works.Add(work);
            await _context.SaveChangesAsync();
        }
    }
}
