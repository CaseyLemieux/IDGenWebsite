using System;
using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IDGenWebsite.Areas.Identity.IdentityHostingStartup))]
namespace IDGenWebsite.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //services.AddDbContext<IDGenWebsiteContext>(options => options.UseMySql(context.Configuration.GetConnectionString("IdGenWebsiteIdentityProd"), MariaDbServerVersion.AutoDetect(context.Configuration.GetConnectionString("IdGenWebsiteIdentityProd"))));
                services.AddDbContext<IDGenWebsiteContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("IdGenWebsiteIdentityProd")));
                /*services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<IDGenWebsiteContext>(); */
                services.AddIdentity<EmployeeModel, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<IDGenWebsiteContext>()
                .AddDefaultTokenProviders().AddDefaultUI().AddRoleManager<RoleManager<IdentityRole>>();

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.Cookie.Name = "IDGenWebsite.Cookie";
                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.SlidingExpiration = true;
                    options.LogoutPath = "/Identity/Account/Logout";
                }); 
            });
        }
    }
}