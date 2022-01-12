using IDGenWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Controllers
{
    public class ReportsController : Controller
    {
        private readonly SchoolContext _schoolContext;

        public ReportsController(SchoolContext context)
        {
            _schoolContext = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> GetNoIdPhotoCount()
        {
            var usersWithNoIdPhoto = await _schoolContext.Users.Where(u => u.IdPicPath == null).CountAsync();
            return Content(usersWithNoIdPhoto.ToString());
        }

        public async Task<IActionResult> GetNoQrCodeCount()
        {
            var usersWithNoQrCode = await _schoolContext.Users.Where(u => u.QrCode == null).CountAsync();
            return Content(usersWithNoQrCode.ToString());
        }

        public async Task<IActionResult> GetInvalidEmailCount()
        {
            var usersWithInvalidEmail = await _schoolContext.Users.Where(u => !u.Email.Contains("@franklincountyschools.org")).CountAsync();
            return Content(usersWithInvalidEmail.ToString());
        }
    }
}
