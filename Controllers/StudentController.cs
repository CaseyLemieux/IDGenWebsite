using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly SchoolContext _context;
        //private readonly IWebHostEnvironment _env;

        public StudentController(ILogger<StudentController> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
            
        }

        public async Task<IActionResult> GetStudentPartial()
        {
            var students = await _context.Students.OrderBy(s => s.LastName).ToListAsync();
            var idOrders = await _context.IDRequests.ToListAsync();
            foreach (var order in idOrders)
            {
                foreach (var student in students)
                {
                    if (order.StudentID.Equals(student.StudentID))
                    {
                        student.IdRequestPrinted = order.HasBeenPrinted;
                    }
                }
            }
            return PartialView("_ViewStudentsPartial", students);
        }

        public async Task<IActionResult> SearchStudents(string searchString)
        {
            var students = from s in _context.Students select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                var names = searchString.Split(" ");
                if(names.Length == 2)
                {
                    students = students.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString) || s.FirstName.Contains(names[0]) && s.LastName.Contains(names[1]) 
                    || s.LastName.Contains(names[0]) && s.FirstName.Contains(names[1]));
                }
                else
                {
                    students = students.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
                }
               
            }
            return PartialView("_ViewStudentsPartial", await students.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students.SingleOrDefaultAsync(s => s.ID == id);
            return PartialView("~/Views/Admin/_EditStudentPartial.cshtml", student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<string> SaveStudentEdits(StudentModel student)
        {
            var dbStudent = await _context.Students.SingleOrDefaultAsync(s => s.ID == student.ID);
            dbStudent.FirstName = student.FirstName;
            dbStudent.LastName = student.LastName;
            dbStudent.QrCode = student.QrCode;
            dbStudent.Email = student.Email;
            dbStudent.DisplayName = student.DisplayName;
            dbStudent.GradeLevel = student.GradeLevel;
            dbStudent.HomeRoomTeacher = student.HomeRoomTeacher;
            dbStudent.HomeRoomTeacherEmail = student.HomeRoomTeacherEmail;
            dbStudent.HasBeenManuallyEdited = true;
            await _context.SaveChangesAsync();
            return JsonConvert.SerializeObject("Success"); ;
        } 
    }
}
