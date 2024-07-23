using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repositories.Identity
{
    public static class AppIdentityDbcontextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {

            var user = new AppUser()
            {
                DisplayName="Mohammed Hossam",
                Email="mohammedhossam199998@gmail.com",
                UserName="Orca",
                PhoneNumber="01125997082"

            };
           await _userManager.CreateAsync(user, "");
        }
            }
    }
}
