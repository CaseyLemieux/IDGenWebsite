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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using IDGenWebsite.Utilities;
using WkHtmlToPdfDotNet.Contracts;
using System.IO.Compression;

namespace IDGenWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SchoolContext _schoolContext;
        private readonly IDGenWebsiteContext _userContext;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly FileHelper _fileHelper;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _env;

        public AdminController(ILogger<AdminController> logger, SchoolContext context, IDGenWebsiteContext userContext, UserManager<EmployeeModel> userManager, IConverter converter
            , IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _schoolContext = context;
            _userContext = userContext;
            _userManager = userManager;
            _converter = converter;
            _fileHelper = new FileHelper(_schoolContext, _converter);
            _env = webHostEnvironment;
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
            await _fileHelper.UploadStudentsAsync(focusFiles);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UploadClassLink(List<IFormFile> classLinkFiles)
        {
            await _fileHelper.UploadQrCodesAsync(classLinkFiles);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UploadIDs(List<IFormFile> idPdfs)
        {
            await _fileHelper.UploadIdsAsync(idPdfs);
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

        public async Task<IActionResult> GetSettingsPartial()
        {
            return PartialView("_SettingsPartial", await _schoolContext.Settings.ToListAsync());
        }

        public async Task<IActionResult> GetIdTemplateSettings()
        {
            return PartialView("_TemplatePartial", await _schoolContext.IdTemplates.ToListAsync());
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
            return JsonConvert.SerializeObject("Failure");
        }

        
        [HttpPost]
        public async Task<IActionResult> SaveID(int id)
        {
            var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.ID == id);
            return File(_fileHelper.GenerateId(student, _env.WebRootPath), "application/pdf",  string.Concat(student.Email, ".pdf"));
            //File(, "application/pdf", string.Concat(student.Email, ".pdf"))
            //return RedirectToAction("ViewStudents", await _context.Students.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> DownloadGradeLevel(string grade) {

            var students = await _schoolContext.Students.Where(s => s.GradeLevel == grade).ToListAsync();
            return File(_fileHelper.GenerateGradeLevel(students, _env.WebRootPath), "application/pdf", string.Concat(grade, ".pdf"));
        }

        [HttpPost]
        public async Task<IActionResult> DownloadByHomeroom()
        {
            var homerooms = _schoolContext.Homerooms.ToList();
            List<ZipItem> zipItems = new List<ZipItem>();
            foreach (HomeroomsModel homeroom in homerooms)
            {
                var students = await _schoolContext.Students.Where(s => s.HomeRoomTeacher == homeroom.Teacher).ToListAsync();
                var classBytes = _fileHelper.GenerateHomeroom(students, _env.WebRootPath);
                if(classBytes != null)
                {
                    string fileName = homeroom.Teacher.Replace(",", "-");
                    ZipItem zipItem = new ZipItem(fileName, classBytes);
                    zipItems.Add(zipItem);
                }
                
            }
            //var zipStream = new MemoryStream();
            byte[] bytes;
            using (var zipStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var zipItem in zipItems)
                    {
                        var entry = zip.CreateEntry(zipItem.Name + ".pdf", CompressionLevel.Fastest);
                        using (var entryStream = entry.Open())
                        {

                            
                            entryStream.Write(zipItem.Content, 0, zipItem.Content.Length);
                        }
                    }
                }
                bytes = zipStream.ToArray();
            }
            return File(bytes, "application/zip", "students.zip");
        }
    }
}
