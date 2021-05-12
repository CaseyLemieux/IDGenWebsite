using IDGenWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Controllers
{
    public class IDOrderController : Controller
    {
        private readonly SchoolContext _schoolContext;
        public IDOrderController(SchoolContext context)
        {
            _schoolContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetIdOrdersPartial()
        {
            return PartialView("_ViewIdOrdersPartial", await _schoolContext.IDRequests.ToListAsync());
        }
    }
}
