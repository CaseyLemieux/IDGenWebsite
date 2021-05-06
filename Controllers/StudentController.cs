using IDGenWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<IActionResult> Index()
        {
            return View("ViewStudents", await _context.Students.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveID(int id)
        {

            var student = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            if (student != null && student.IdPic != null)
            {
                return File(student.IdPic, "application/pdf", string.Concat(student.Email, ".pdf"));
            }

            return RedirectToAction("ViewStudents", await _context.Students.ToListAsync());
        }
    }
}
