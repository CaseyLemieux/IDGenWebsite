using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class StudentModel
    {
        public int ID { get; set; }
        [Display(Name = "Student ID:")]
        public string StudentID { get; set; }
        [Display(Name ="First Name:")]
        public string FirstName { get; set; }
        [Display(Name ="Last Name:")]
        public string LastName { get; set; }
        [Display(Name = "Email:")]
        public string Email { get; set; }
        [Display(Name ="Display Name:")]
        public string DisplayName { get; set; }
        [Display(Name ="QR Code:")]
        public string QrCode { get; set; }
        [Display(Name ="Grade Level:")]
        public string GradeLevel { get; set; }
        [Display(Name ="ID Picture:")]
        public string IdPicPath { get; set; }
        [Display(Name = "Active Status:")]
        public bool IsActive { get; set; }
        [Display(Name = "Delete Status:")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Enrollment Start Date:")]
        public DateTime EnrollmentStartDate { get; set; }
        [Display(Name = "Home Room Teacher:")]
        public string HomeRoomTeacher { get; set; }
        [Display(Name = "Home Room Teacher Email:")]
        public string HomeRoomTeacherEmail { get; set; }
    }
}
