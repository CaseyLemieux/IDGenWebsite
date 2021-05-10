using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                } else
                {
                    return RedirectToAction("Dashboard", "User");
                }
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }
        }
    }
}
