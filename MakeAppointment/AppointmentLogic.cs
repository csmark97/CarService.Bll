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
using CarService.Dal.Manager;
using System.Linq.Expressions;
using CarService.Bll.EmailService;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CarService.Bll.MakeAppointment
{
    public class AppointmentLogic
    {
        private static ApplicationEntityManager _applicationEntityManager;

        public AppointmentLogic(CarServiceDbContext context)
        {
            _applicationEntityManager = new ApplicationEntityManager(context);
        }

        public static async Task<SubTask> GetSubTaskByIdAsync(int id)
        {
            return await ApplicationEntityManager.GetSubTaskByIdAsync(id);
        }

        public static IDictionary<DayOfWeek, OpeningDay> GetOpening(Opening opening)
        {
            return OpeningHandler.GetOpening(opening);
        }

        public static async Task<IList<SelectListItem>> GetCarsByIdAsync(string userId)
        {
            IList<Car> cars = await ApplicationEntityManager.GetCarsByUserIdAsync(userId);

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

        private static async Task<int?> ServiceExistsAsync(int carId)
        {
            bool carHasService = ApplicationEntityManager.CarHasService(carId);

            if (carHasService)
            {
                IList<Service> services = await ApplicationEntityManager.GetServicesByCarIdAsync(carId);

                foreach (var service in services)
                {
                    foreach (var work in service.Works)
                    {
                        if (!work.State.Name.Equals("Finished") && !work.State.Name.Equals("FinishedAndPaid"))
                        {
                            return service.Id;
                        }
                    }
                }
            }

            return null;
        }

        public static async Task<IDictionary<DayOfWeek, Dictionary<DateTime, bool>>> GetFinalOpeningAsync(SubTask subTask)
        {
            IList<WorkerUser> workerUsers;
            workerUsers = await ApplicationEntityManager.GetWorkerUsersByCompanyIdAsync(subTask.CompanyUserId);

            return OpeningHandler.GetFinalOpening(workerUsers, subTask);
        }

        public static async Task<Work> MakeAppointmentAsync(DateTime appointment, int carId, SubTask subTask)
        {
            subTask = await ApplicationEntityManager.GetSubTaskByIdAsync(subTask.Id);

            IList<WorkerUser> workerUsers = await ApplicationEntityManager.GetWorkerUsersByCompanyIdAsync(subTask.CompanyUserId);

            workerUsers.Shuffle();

            WorkerUser workerForTheJob = WorkerHandler.GetWorkerForTheJob(workerUsers, appointment, appointment.AddMinutes(subTask.EstimtedTime));

            int? openServiceId = await ServiceExistsAsync(carId);

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

                await ApplicationEntityManager.SaveServiceAsync(service);
            }
            else
            {
                service = await ApplicationEntityManager.GetServcieByIdAsync(openServiceId.Value);

                if (service.Works.ElementAt(0).SubTask.CompanyUserId == subTask.CompanyUserId)
                {
                    service.TotalPrice = 0;
                    foreach (var w in service.Works)
                    {
                        service.TotalPrice += w.Price;
                    }
                    service.TotalPrice += subTask.EstimatedPrice;

                    if (appointment < service.StartingTime)
                    {
                        service.StartingTime = appointment;
                    }

                    if (appointment.AddMinutes(subTask.EstimtedTime) > service.EndTime)
                    {
                        service.EndTime = appointment.AddMinutes(subTask.EstimtedTime);
                    }

                    ApplicationEntityManager.ModifyService(service);
                }
                else
                {
                    service = new Service
                    {
                        StartingTime = appointment,
                        EndTime = appointment.AddMinutes(subTask.EstimtedTime),
                        TotalPrice = subTask.EstimatedPrice,
                        CarId = carId
                    };

                    await ApplicationEntityManager.SaveServiceAsync(service);
                }
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

            await ApplicationEntityManager.SaveWorkAsync(work);

            return work;
        }
    }
}
