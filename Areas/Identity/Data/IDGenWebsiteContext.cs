using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDGenWebsite.Data
{
    public class IDGenWebsiteContext : IdentityDbContext<EmployeeModel>
    {
        public IDGenWebsiteContext(DbContextOptions<IDGenWebsiteContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeeModel> EmployeeModel { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //Unique GUIDs for the user and role
            /*const string ADMIN_ID = "143d041e-9118-4804-b425-3bdc8f5698f6";
            const string ROLE_ID = "3167bb56-7ab7-4651-8856-bfe7088b7f77";

            //Seed the Admin Role
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = ROLE_ID, Name = "Admin", NormalizedName = "admin" });

            var hasher = new PasswordHasher<EmployeeModel>();
            builder.Entity<EmployeeModel>().HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "lemieuxcasey@gmail.com",
                NormalizedUserName = "lemieuxcasey@gmail.com",
                Email = "lemieuxcasey@gmail.com",
                NormalizedEmail = "lemieuxcasey@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "*CHevy6969*"),
                SecurityStamp = string.Empty

            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            }); */

           
        }
    }
}
