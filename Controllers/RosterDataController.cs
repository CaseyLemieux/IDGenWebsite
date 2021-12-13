using IDGenWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Orgs()
        {
            var orgs = await _schoolContext.Orgs.ToListAsync();
            return View(orgs);
        }
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _schoolContext.AcademicSessions.ToListAsync();
            return View(sessions);
        }
        public async Task<IActionResult> Users()
        {
            var users = await _schoolContext.Users.ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> Courses()
        {
            var courses = await _schoolContext.Courses.ToListAsync();
            return View(courses);
        }
        public async Task<IActionResult> Classes()
        {
            var classes = await _schoolContext.Classes.ToListAsync();
            return View(classes);
        }
        public async Task<IActionResult> Enrollments()
        {
            var enrollments = await _schoolContext.Enrollments.ToListAsync();
            return View(enrollments);
        }
    }
}
