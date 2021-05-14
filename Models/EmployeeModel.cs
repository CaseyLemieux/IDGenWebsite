using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
        [NotMapped]
        [Display(Name = "Role:")]
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>
        {
            new SelectListItem{Value = "Admin", Text = "Admin"},
            new SelectListItem{Value = "User", Text = "User"}
        };
        [NotMapped]
        public string SelectedRole { get; set; }
    }
}
