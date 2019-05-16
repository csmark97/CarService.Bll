using CarService.Dal;
using CarService.Dal.Entities;
using CarService.Dal.Manager;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Bll.EmailService
{
    public class EmailLogic
    {
        private readonly ApplicationEntityManager _applicationEntityManager;
        private readonly IEmailSender _emailSender;

        public EmailLogic(CarServiceDbContext carServiceDbContext, IEmailSender emailSender)
        {
            _applicationEntityManager = new ApplicationEntityManager(carServiceDbContext);
            _emailSender = emailSender;
        }

        public async Task SendStatusChangeEmailAsync(WorkerUser workerUser, Work work)
        {
            var email = work.Service.Car.ClientUser.Email;
            var clientName = work.Service.Car.ClientUser.Name;
            var car = work.Service.Car.Brand + " " + work.Service.Car.Model + " " + work.Service.Car.YearOfManufacture;
            var workDeatils = work.SubTask.Name + " (" + work.Price + " Ft)";
            var state = work.State.NameHungarian;            

            var subject = "Állapotváltozás - noreply";

            string message;

            if (work.StateId == 1)
            {
                message = ($"Tisztelt {clientName}!<br /><br />{car} autójához " +
                    $"tartozó {workDeatils} munka állapota megváltozott!<br /><br />Új állapot: {state}" +
                    $"<br /><br /><br />Ez azt jelenti, hogy önnek be kell lépnie felületünkre és elfogadni valamilyen módosítást, " +
                    $"amely legtöbbször a munka árára vonatkozik. Erről pontosabb tájékoztatás a felületen a " +
                    $"munkához tartozó üzenetek között talál.<br /><br />" +
                    $"Üdvözlettel,<br />{work.SubTask.Name}");
            }
            else if (work.StateId == 2)
            {
                message = ($"Tisztelt {clientName}!<br /><br />{car} autójához " +
                    $"tartozó {workDeatils} munkát rögzítettük!<br /><br />Állapot: {state}.<br /><br />" +
                    $"Üdvözlettel,<br />{work.SubTask.Name}");
            }
            else
            {
                message = ($"Tisztelt {clientName}!<br /><br />{car} autójához " +
                    $"tartozó {workDeatils} munka állapota megváltozott!<br /><br />Új állapot: {state}.<br /><br />" +
                    $"Üdvözlettel,<br />{work.SubTask.Name}");
            }


            await _emailSender.SendEmailAsync(email, subject, message);
        }

        public async Task SendStatusChangeEmailAsync(Service service)
        {
            var email = service.Car.ClientUser.Email;
            var subject = "Szervíz fizetve - noreply";
            var message = $"Tisztelt {service.Car.ClientUser.Name}!<br /><br />" +
                $"Értesítjük, hogy a szervíz fizetve lett!";

            await _emailSender.SendEmailAsync(email, subject, message);
        }

        public async Task SendNotificationAsync(Work work, string message)
        {
            var email = work.Service.Car.ClientUser.Email;           

            var subject = "Értesítés - noreply";            

            await _emailSender.SendEmailAsync(email, subject, message);
        }
    }
}
