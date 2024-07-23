﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extentions
{
    public static class UserManagerExtentions
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User) {
        
            var userEmail= User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u=>u.Email==userEmail);

            return user;
        }
    }
}