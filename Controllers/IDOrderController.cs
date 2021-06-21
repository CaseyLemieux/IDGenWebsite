using IDGenWebsite.Data;
using IDGenWebsite.Models;
using IDGenWebsite.Utilities;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<EmployeeModel> _userManager;
        public IDOrderController(SchoolContext context, UserManager<EmployeeModel> userManager)
        {
            _schoolContext = context;
            _userManager = userManager;
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
            List<EmployeeModel> admins = (List<EmployeeModel>)await _userManager.GetUsersInRoleAsync("Admin");
            await _emailHelper.SendNewOrderRequestEmail(admins);
            return JsonConvert.SerializeObject("Success");
        }
    }
}
