using IDGenWebsite.Data;
using IDGenWebsite.Models;
using IDGenWebsite.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Controllers
{
    public class IDOrderController : Controller
    {
        private readonly SchoolContext _schoolContext;
        private readonly EmailHelper _emailHelper;
        public IDOrderController(SchoolContext context)
        {
            _schoolContext = context;
            //Need to move this API Key to the database
            _emailHelper = new EmailHelper("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");
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetIdOrdersPartial()
        {
            return PartialView("_ViewIdOrdersPartial", await _schoolContext.IDRequests.ToListAsync());
        }

        public async Task<string> RequestIdPrint(int id)
        {
            var student = await _schoolContext.Students.FirstOrDefaultAsync(s => s.ID == id);
            var idOrder = new IDRequestModel()
            {
                StudentID = student.StudentID,
                UserName = User.Identity.Name,
                DateCreated = DateTime.Now,
                Cost = 1.50
            };
            _schoolContext.IDRequests.Add(idOrder);
            _schoolContext.SaveChanges();
            await _emailHelper.Send();
            return JsonConvert.SerializeObject("Success");
        }
    }
}
