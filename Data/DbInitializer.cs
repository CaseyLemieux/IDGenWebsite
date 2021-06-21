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
            if (context.Database.EnsureCreated())
            {
                SeedDefaultSettings(context);
                SeedDefaultTemplate(context);
            }
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

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        private static void SeedDefaultSettings(SchoolContext context)
        {
            //Id Cost Setting
            SettingModel idCost = new SettingModel
            {
                SettingName = "Id Cost",
                SettingValue = "Not Set",
                SettingDescription = "The cost to print a new student ID",
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.UnixEpoch,
                IsDeleted = false
            };

            //Setting for EmailApiKey
            SettingModel emailApiKey = new SettingModel
            {
                SettingName = "Email Api Key",
                SettingValue = "Not Set",
                SettingDescription = "Api Key to allow for Email Notifications",
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.UnixEpoch,
                IsDeleted = false
            };

            //Setting For Email Notifications Enabled
            SettingModel emailNotification = new SettingModel
            {
                SettingName = "Email Notification",
                SettingValue = "Not Set",
                SettingDescription = "Setting for turning on or off Email Notifications",
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.UnixEpoch,
                IsDeleted = false
            };

            //Setting for Daily Sync Enabled
            SettingModel dailySyncEnabled = new SettingModel
            {
                SettingName = "Daily Sync",
                SettingValue = "Not Set",
                SettingDescription = "Setting for turning on or off the Daily Sync",
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.UnixEpoch,
                IsDeleted = false
            };

            //Setting for Daily Sync Time
            SettingModel dailySyncTime = new SettingModel
            {
                SettingName = "Daily Sync Time",
                SettingValue = "Not Set",
                SettingDescription = "The time for the Daily Sync to occur",
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.UnixEpoch,
                IsDeleted = false
            };

            context.Settings.Add(idCost);
            context.Settings.Add(emailNotification);
            context.Settings.Add(emailApiKey);
            context.Settings.Add(dailySyncEnabled);
            context.Settings.Add(dailySyncTime);
            context.SaveChanges();

        }

        private static void SeedDefaultTemplate(SchoolContext context)
        {
            IdTemplate defaultTemplate = new IdTemplate
            {
                TemplateName = "Default Template",
                FrontTopColor = "#c20606",
                FrontMiddleColor = "white",
                FrontBottomColor = "white",
                BackTopColor = "white",
                BackMiddleColor = "white",
                BackBottomColor = "white",
                LogoPath = "Null", //TODO: Change this to a default logo that is included with the system
                FrontTopLabel = "Default Template",
                BackTopLabel = "Default Template",
                BackInfoLineOne = "Default Template",
                BackInfoLineTwo = "Default Template",
                BackInfoLineThree = "Default Template",
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.UnixEpoch,
                IsDeleted = false

            };
            context.IdTemplates.Add(defaultTemplate);
            context.SaveChanges();
        }
    }
}
