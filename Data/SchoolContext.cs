using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDGenWebsite.Models;

namespace IDGenWebsite.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }

        //Adding the non OneRoster Tables to the database.
        public DbSet<IDRequestModel> IDRequests { get; set; }
        public DbSet<SettingModel> Settings { get; set; }
        public DbSet<IdTemplate> IdTemplates { get; set; }

        //Add the OneRoster Tables to the Database
        public DbSet<Organizations> Orgs { get; set; }
        public DbSet<AcademicSessions> AcademicSessions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }

    }
}
