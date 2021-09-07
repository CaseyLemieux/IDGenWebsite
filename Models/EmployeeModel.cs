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
        [Required]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }
        [Display(Name = "School ID:")]
        public string SchoolID { get; set; }
        [Display(Name = "Teacher ID:")]
        public string TeacherID { get; set; }
        [Display(Name = "Active Status:")]
        public bool IsActive { get; set; }
        [Display(Name = "Delete Status:")]
        public bool IsDeleted { get; set; }
        [Required]
        [NotMapped]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
        [Required]
        [NotMapped]
        [Display(Name = "Role:")]
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>
        {
            new SelectListItem{Value = "Admin", Text = "Admin"},
            new SelectListItem{Value = "User", Text = "User"},
            new SelectListItem{Value = "Teacher", Text = "Teacher"}
        };
        [NotMapped]
        public string SelectedRole { get; set; }
    }
}
