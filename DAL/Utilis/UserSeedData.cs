using DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utilis
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "sanabel",
                    Email = "sanabel@gmail.com",
                    FullName = "sanabel barham",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser
                {
                    UserName = "malakoot",
                    Email = "malakoot@gmail.com",
                    FullName = "malakoot barham",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser
                {
                    UserName = "sabri",
                    Email = "sabri@gmail.com",
                    FullName = "sabri barham",
                    EmailConfirmed = true
                };
                //the second part is the password
             var r1=   await _userManager.CreateAsync(user1, "Dmsysa@310877");
              var r2=  await _userManager.CreateAsync(user2, "Sanabel@2005Queen");
              var r3=  await _userManager.CreateAsync(user3, "Sabri@2134King");
               
                 await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                  await _userManager.AddToRoleAsync(user2, "Admin");
                await _userManager.AddToRoleAsync(user3, "User");
               


            }


        }
    }
}
