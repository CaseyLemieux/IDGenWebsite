using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class IDRequest
    {
        [Display(Name = "Print Request ID:" )]
        public int ID { get; set; }
        [Display(Name = "Student ID:")]
        public int StudentID { get; set; }
        [Display(Name = "User ID:")]
        public int UserID { get; set; }
        [Display(Name = "Print Status:")]
        public bool HasBeenPrinted { get; set; }
        [Display(Name = "Date Created:")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Date Printed:")]
        public DateTime DatePrinted { get; set; }
        [Display(Name = "Cost:")]
        public double Cost { get; set; }
    }
}
