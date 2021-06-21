using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            CreateIdentityDBIfNotExists(host);
            SeedData(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //Create the Student Database if it does not already exsist.
                    //After creating the Database seed default settings and Id Template.
                    var context = services.GetRequiredService<SchoolContext>();
                    DbInitializer.Initialize(context);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the Student DB.");
                }
            }
        }

        public static void CreateIdentityDBIfNotExists(IHost host)
        {
            using(var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<IDGenWebsiteContext>();
                    DbInitializer.InitializeIdentityDB(context);
                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "AN error occured creating the Identity DB");
                }
            }
        }

        public static void SeedData(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<EmployeeModel>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    DbInitializer.SeedData(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An Error occured seeding the data");
                }
            }
        }
    }
}
