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

        public DbSet<StudentModel> Students { get; set; }
        public DbSet<IDRequestModel> IDRequests { get; set; }
        public DbSet<SettingModel> Settings { get; set; }
    }
}
