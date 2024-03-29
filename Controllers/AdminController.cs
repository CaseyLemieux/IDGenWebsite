﻿using ExcelDataReader;
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
using System.Threading;

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

        public AdminController(ILogger<AdminController> logger, SchoolContext context, IDGenWebsiteContext userContext, UserManager<EmployeeModel> userManager, IConverter converter, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _schoolContext = context;
            _userContext = userContext;
            _userManager = userManager;
            _converter = converter;
            _env = webHostEnvironment;
            _fileHelper = new FileHelper(_schoolContext, _converter, _env.WebRootPath);
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
            return File(_fileHelper.GenerateId(student), "application/pdf",  string.Concat(student.Email, ".pdf"));
            //File(, "application/pdf", string.Concat(student.Email, ".pdf"))
            //return RedirectToAction("ViewStudents", await _context.Students.ToListAsync());
        }

        public async Task<IActionResult> DownloadQrCode(int id)
        {
            var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.ID == id);
            return File(_fileHelper.GenerateQrCode(student), "application/pdf", string.Concat(student.Email, ".pdf"));
        }

        [HttpPost]
        public async Task<IActionResult> DownloadQrsByHomeroom()
        {
            var homerooms = await _schoolContext.Homerooms.ToListAsync();
            List<ZipItem> zipItems = new List<ZipItem>();
            foreach (HomeroomsModel homeroom in homerooms)
            {
                var students = await _schoolContext.Students.Where(s => s.HomeRoomTeacher == homeroom.Teacher).ToListAsync();
                string fileName = homeroom.Teacher.Replace(",", "-");
                var classBytes = _fileHelper.GenerateQrCodes(students);
                if (classBytes != null && classBytes.Length > 0)
                {
                    ZipItem zipItem = new ZipItem(fileName, classBytes);
                    zipItems.Add(zipItem);
                }
            }
            return File(_fileHelper.GenerateZipFile(zipItems), "application/zip", "QrCodesByHomeroom.zip");
        }

        [HttpPost]
        public async Task<IActionResult> DownloadIdsByGradeLevel(string grade) {

            var students = await _schoolContext.Students.Where(s => s.GradeLevel == grade).ToListAsync();
            List<ZipItem> zipItems = new List<ZipItem>();
            foreach(StudentModel student in students)
            {
                var bytes = _fileHelper.GenerateId(student);
                if (bytes != null && bytes.Length > 0)
                {
                    string fileName = student.Email;
                    ZipItem zipItem = new ZipItem(fileName, bytes);
                    zipItems.Add(zipItem);
                }
            }
            return File(_fileHelper.GenerateZipFile(zipItems), "application/zip", string.Concat("Grade",grade,"IDs", ".zip"));
        }

        
        
        [HttpPost]
        public async Task<IActionResult> DownloadIdsByHomeroom()
        {
            var homerooms = _schoolContext.Homerooms.ToList();
            Dictionary<string, List<ZipItem>> teacherList = new Dictionary<string, List<ZipItem>>();
            foreach (HomeroomsModel homeroom in homerooms)
            {
                var students = await _schoolContext.Students.Where(s => s.HomeRoomTeacher == homeroom.Teacher).ToListAsync();
                List<ZipItem> zipItems = new List<ZipItem>();
                string fileName = homeroom.Teacher.Replace(",", "-");
                foreach (StudentModel student in students){
                    var classBytes = _fileHelper.GenerateId(student);
                    if (classBytes != null && classBytes.Length > 0)
                    { 
                        ZipItem zipItem = new ZipItem(student.Email, classBytes);
                        zipItems.Add(zipItem);
                    }
                }
                teacherList.Add(fileName, zipItems);
            }
            return File(_fileHelper.GenerateZipFile(teacherList), "application/zip", "IDsByHomeroom.zip");
        } 
    }
}
