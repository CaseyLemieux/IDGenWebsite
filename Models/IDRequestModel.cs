using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class IDRequestModel
    {
        [Display(Name = "Print Request ID:" )]
        public int ID { get; set; }
        [Display(Name = "Student ID:")]
        public int StudentID { get; set; }
        [Display(Name = "Created By:")]
        public string UserName { get; set; }
        [Display(Name = "Date Created:")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Date Printed:")]
        public DateTime DatePrinted { get; set; }
        [Display(Name = "Cost:")]
        public double Cost { get; set; }
        [Display(Name = "Print Status:")]
        public bool HasBeenPrinted { get; set; }
        [Display(Name = "Delete:")]
        public bool IsDeleted { get; set; }
    }
}
