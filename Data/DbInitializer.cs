using IDGenWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();
        }

        public static void InitializeIdentityDB(IDGenWebsiteContext context)
        {
            context.Database.EnsureCreated();
        }

        public static void SeedData(UserManager<EmployeeModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<EmployeeModel> userManager)
        {
            if(userManager.FindByEmailAsync("lemieuxcasey@gmail.com").Result == null)
            {
                EmployeeModel user = new EmployeeModel();
                user.FirstName = "Casey";
                user.LastName = "Lemieux";
                user.UserName = "lemieuxcasey@gmail.com";
                user.Email = "lemieuxcasey@gmail.com";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "*CHevy6969*").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
