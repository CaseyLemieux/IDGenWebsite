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
using Microsoft.AspNetCore.Hosting;

namespace IDGenWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RosterDataController : Controller
    {
        private readonly ILogger<RosterDataController> _logger;
        private readonly SchoolContext _schoolContext;
        private readonly IWebHostEnvironment _env;

        public RosterDataController(ILogger<RosterDataController> logger, SchoolContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _schoolContext = context;
            _env = webHostEnvironment;
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
            var sessions = await _schoolContext.AcademicSessions.Include(a => a.Parent).ToListAsync();

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
                else
                {
                    string idPhotoBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(_env.WebRootPath + "/noUser96.png"));
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

            var dataTableResponse = new DataTableResponse
            {
                RecordsTotal = classes.Count(),
                RecordsFiltered = 10,
                Data = classes.ToArray()
            };
            return dataTableResponse;
        }

        [HttpGet]
        public async Task<string> GetClassUsers(string classSourcedId)
        {
            //Finally we send those user back to the frontend
            //First We Need to get the class and its enrollments with users
            var selectedClass = await _schoolContext.Classes.Include(a => a.AcademicSessions).Include(e => e.Enrollments).ThenInclude(e => e.User).SingleOrDefaultAsync(c => c.ClassSourcedId == Guid.Parse(classSourcedId));
            List<Users> classUsers = new List<Users>();
            foreach(var enrollment in selectedClass.Enrollments)
            {
                var user = enrollment.User;
                classUsers.Add(user);
            }
            var response = new DataTableResponse
            {
                RecordsTotal = classUsers.Count,
                RecordsFiltered = 10,
                Data = classUsers.ToArray()
            };
            string jsonData = JsonConvert.SerializeObject(response, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return jsonData;

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


        public async Task<IActionResult> GetOrgsCount()
        {
            var orgCount = await _schoolContext.Orgs.CountAsync();
            return Content(orgCount.ToString());
        }

        public async Task<IActionResult> GetSessionsCount()
        {
            var sessionsCount = await _schoolContext.AcademicSessions.CountAsync();
            return Content(sessionsCount.ToString());
        }

        public async Task<IActionResult> GetUsersCount()
        {
            var usersCount = await _schoolContext.Users.CountAsync();
            return Content(usersCount.ToString());
        }

        public async Task<IActionResult> GetCoursesCount()
        {
            var coursesCount = await _schoolContext.Courses.CountAsync();
            return Content(coursesCount.ToString());
        }

        public async Task<IActionResult> GetClassesCount()
        {
            var classesCount = await _schoolContext.Classes.CountAsync();
            return Content(classesCount.ToString());
        }

        public async Task<IActionResult> GetEnrollmentsCount()
        {
            var enrollmentsCount = await _schoolContext.Enrollments.CountAsync();
            return Content(enrollmentsCount.ToString());
        }

        
    }
}
