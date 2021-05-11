using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class EmployeeModel : IdentityUser
    {
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }
        [Display(Name = "Active Status:")]
        public bool IsActive { get; set; }
        [Display(Name = "Delet Status:")]
        public bool IsDeleted { get; set; }
    }
}
