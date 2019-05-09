using CarService.Dal;
using CarService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace CarService.Bll.User
{
    public class AppUserManager
    {
        private static CarServiceDbContext _context;

        private const string _nameIdentifierString = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public AppUserManager(CarServiceDbContext context)
        {
            _context = context;
        }              

        public static async Task<ClientUser> GetUserAsync(ClaimsPrincipal User)
        {
            var userId = User.Claims.Single(c => c.Type == _nameIdentifierString).Value;
            return await _context.ClientUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
