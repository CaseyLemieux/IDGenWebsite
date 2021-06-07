using ExcelDataReader;
using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using IronPdf;
using UglyToad.PdfPig;
using QRCoder;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Writer;
using UglyToad.PdfPig.Core;
using IDGenWebsite.Utilities;

namespace IDGenWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SchoolContext _schoolContext;
        private readonly IDGenWebsiteContext _userContext;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly FileHelper fileHelper;
        //private readonly IWebHostEnvironment _env;

        public AdminController(ILogger<AdminController> logger, SchoolContext context, IDGenWebsiteContext userContext, UserManager<EmployeeModel> userManager)
        {
            _logger = logger;
            _schoolContext = context;
            _userContext = userContext;
            _userManager = userManager;
            
            fileHelper = new FileHelper(_schoolContext);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        

        
        [HttpPost]
        public async Task<IActionResult> UploadFocus(List<IFormFile> focusFiles)
        {
            await fileHelper.UploadStudentsAsync(focusFiles);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UploadClassLink(List<IFormFile> classLinkFiles)
        {
            await fileHelper.UploadQrCodesAsync(classLinkFiles);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UploadIDs(List<IFormFile> idPdfs)
        {
            await fileHelper.UploadIdsAsync(idPdfs);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }

        

        public async Task<IActionResult> GetUsersPartial()
        {
            return PartialView("_ViewUsersPartial", await _userContext.EmployeeModel.ToListAsync());
        }

        public IActionResult GetUploadFilesPartial()
        {
            return PartialView("_UploadFilesPartial");
        }

        public IActionResult GetCreateUserPartial()
        {
            return PartialView("_CreateUserPartial", new EmployeeModel());
        }
        [HttpPost]
        public string CreateUser(EmployeeModel employee)
        {
            employee.EmailConfirmed = true;
            employee.UserName = employee.Email;
            if (ModelState.IsValid)
            {
                if(_userManager.FindByEmailAsync(employee.Email).Result == null)
                {
                    IdentityResult result = _userManager.CreateAsync(employee, employee.Password).Result;
                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(employee, employee.SelectedRole).Wait();
                        return JsonConvert.SerializeObject("Success");
                    }
                    else
                    {
                        return JsonConvert.SerializeObject("Failure");
                    }
                } 
            }
            //If we got this far something went wrong
            return JsonConvert.SerializeObject("Failuer");
        } 



    }
}
