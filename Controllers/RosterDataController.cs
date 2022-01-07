using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace IDGenWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RosterDataController : Controller
    {
        private readonly ILogger<RosterDataController> _logger;
        private readonly SchoolContext _schoolContext;

        public RosterDataController(ILogger<RosterDataController> logger, SchoolContext context)
        {
            _logger = logger;
            _schoolContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Orgs()
        {
            Organizations org = new Organizations();
            return View(org);
        }
        public IActionResult Sessions()
        {
            AcademicSessions session = new AcademicSessions();
            return View(session);
        }
        public IActionResult Users()
        {
            Users user = new Users();
            return View(user);
        }
        public IActionResult Courses()
        {
            Courses course = new Courses();
            return View(course);
        }
        public IActionResult Classes()
        {
            Classes classes = new Classes();
            return View(classes);
        }
        public IActionResult Enrollments()
        {
            Enrollments enrollment = new Enrollments();
            return View(enrollment);
        }
        [HttpGet]
        public async Task<ActionResult<DataTableResponse>> GetOrgs()
        {
            var orgs = await _schoolContext.Orgs.ToListAsync();

            return new DataTableResponse
            {
                RecordsTotal = orgs.Count(),
                RecordsFiltered = 10,
                Data = orgs.ToArray()
            };
        }

        [HttpGet]
        public async Task<ActionResult<DataTableResponse>> GetSessions()
        {
            var sessions = await _schoolContext.AcademicSessions.ToListAsync();

            return new DataTableResponse
            {
                RecordsTotal = sessions.Count(),
                RecordsFiltered = 10,
                Data = sessions.ToArray()
            };
        }

        [HttpGet]
        public async Task<ActionResult<DataTableResponse>> GetUsers()
        {
            var users = await _schoolContext.Users.Include(g => g.Grades).ToListAsync();
            foreach(var user in users)
            {
                if(user.IdPicPath != null)
                {
                    string idPhotoBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(user.IdPicPath));
                    user.IdBase64 = idPhotoBase64;
                }
            }
            return new DataTableResponse
            {
                RecordsTotal = users.Count(),
                RecordsFiltered = 10,
                Data = users.ToArray()
            };
        }

        [HttpGet]
        public async Task<ActionResult<DataTableResponse>> GetCourses()
        {
            var courses = await _schoolContext.Courses.ToListAsync();

            return new DataTableResponse
            {
                RecordsTotal = courses.Count(),
                RecordsFiltered = 10,
                Data = courses.ToArray()
            };
        }

        [HttpGet]
        public async Task<ActionResult<DataTableResponse>> GetClasses()
        {
            var classes = await _schoolContext.Classes.ToListAsync();

            return new DataTableResponse
            {
                RecordsTotal = classes.Count(),
                RecordsFiltered = 10,
                Data = classes.ToArray()
            };
        }

        [HttpGet]
        public async Task<ActionResult<DataTableResponse>> GetEnrollments()
        {
            var enrollments = await _schoolContext.Enrollments.ToListAsync();

            return new DataTableResponse
            {
                RecordsTotal = enrollments.Count(),
                RecordsFiltered = 10,
                Data = enrollments.ToArray()
            };
        }

        /*[HttpGet]
        public async Task<IActionResult> GetStudentPhoto(string sourcedId)
        {

        } */
    }
}
