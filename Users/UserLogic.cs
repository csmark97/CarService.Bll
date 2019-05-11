using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using CarService.Dal.Manager;

namespace CarService.Bll.Users
{
    public class UserLogic
    {

        private const string _nameIdentifierString = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        private readonly ApplicationUserManager _applicationUserManager;

        public UserLogic(CarServiceDbContext context)
        {
            _applicationUserManager = new ApplicationUserManager(context);
        }              

        public static async Task<ClientUser> GetUserAsync(ClaimsPrincipal User)
        {
            var userId = User.Claims.Single(c => c.Type == _nameIdentifierString).Value;
            return await ApplicationUserManager.GetUserAsync(userId);
        }
    }
}
