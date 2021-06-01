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

namespace IDGenWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SchoolContext _schoolContext;
        private readonly IDGenWebsiteContext _userContext;
        private readonly UserManager<EmployeeModel> _userManager;
        //private readonly IWebHostEnvironment _env;

        public AdminController(ILogger<AdminController> logger, SchoolContext context, IDGenWebsiteContext userContext, UserManager<EmployeeModel> userManager)
        {
            _logger = logger;
            _schoolContext = context;
            _userContext = userContext;
            _userManager = userManager;
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
        
        private async Task ParseClasslinkFile(string fileName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.Email == reader.GetValue(2).ToString());
                        if(student != null)
                        {
                            student.DisplayName = reader.GetValue(3).ToString();
                            student.QrCode = reader.GetValue(4).ToString();
                        }
                    }
                    _schoolContext.SaveChanges();
                }
            }
            //RedirectToAction("ViewStudents");
        }

        private async Task TestExcel(string fileName)
        {
            //List<StudentModel> students = new List<StudentModel>();

            //var fileName = "./Focus Students May.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        StudentModel student = new StudentModel();
                        student.StudentID = reader.GetValue(0).ToString();
                        student.LastName = reader.GetValue(1).ToString();
                        student.FirstName = reader.GetValue(2).ToString();
                        student.Email = reader.GetValue(3).ToString();
                        student.GradeLevel = reader.GetValue(4).ToString();

                        var dbEntry = await _schoolContext.Students.FirstOrDefaultAsync(s => s.StudentID == student.StudentID);
                        if(dbEntry == null)
                        {
                             _schoolContext.Add(student);
                        }
                        /*_context.Add(new StudentModel
                        {
                            StudentID = reader.GetValue(0).ToString(),
                            LastName = reader.GetValue(1).ToString(),
                            FirstName = reader.GetValue(2).ToString(),
                            Email = reader.GetValue(3).ToString(),
                            GradeLevel = reader.GetValue(4).ToString()
                        }); */
                    }
                    _schoolContext.SaveChanges();
                }
            }
            
        }
        /*public IActionResult UploadStudents()
        {
            return View("UploadFiles");
        } */
        [HttpPost]
        public async Task<IActionResult> UploadFocus(ICollection<IFormFile> focusFiles)
        {
            var folderName = "Uploads";
            var fullPath = "";

            if (focusFiles != null && focusFiles.Count != 0)
            {
                foreach (var formFile in focusFiles)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory());
                    fullPath = Path.Combine(pathToSave, fileName);
                    using(var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //var fileName = filePaths.FirstOrDefault();
            await TestExcel(fullPath);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UploadClassLink(List<IFormFile> classLinkFiles)
        {
            var folderName = "Uploads";
            var fullPath = "";
            if (classLinkFiles != null)
            {
                foreach (var formFile in classLinkFiles)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory());
                    fullPath = Path.Combine(pathToSave, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //var fileName = filePaths.FirstOrDefault();
            await ParseClasslinkFile(fullPath);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UploadIDs(List<IFormFile> idPdfs)
        {
            var folderName = "Uploads";
            var fullPath = "";
            if(idPdfs != null)
            {
                foreach(var formFile in idPdfs)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory());
                    fullPath = Path.Combine(pathToSave, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }
            await ParseFocusPDF(fullPath);
            return PartialView("_ViewStudentsPartial", await _schoolContext.Students.ToListAsync());
        }

        private async Task ParseFocusPDF(string path)
        {
            /*
            //Create the QrCode generator and get the pdf from the uploaded files
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            PdfDocument pdf = PdfDocument.FromFile(path);
            //Loop through the pages and extract the text to match id pages with students
            for(int i = 0; i < pdf.PageCount; i++)
            {
                string text = pdf.ExtractTextFromPage(i);
                string[] textArray = text.Split(new string[] { ",", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if(textArray.Length == 3)
                {
                    string id = textArray[2];
                    var student = await _schoolContext.Students.FirstOrDefaultAsync(s => s.StudentID == id);
                    if(student != null)
                    {
                        //Save the page as their ID picture
                        PdfDocument page = pdf.CopyPage(i);
                        

                        //Generate their QR Code if their QR Code field isnt null
                        //TODO Redo this later to a seperate method that can be called on demand. 
                        if (student.QrCode != null)
                        {
                            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                            QRCode qRCode = new QRCode(qRCodeData);
                            Bitmap qrCodeImage = qRCode.GetGraphic(15);
                            PdfDocument qrDoc = ImageToPdfConverter.ImageToPdf(qrCodeImage);
                            HtmlHeaderFooter footer = new HtmlHeaderFooter();
                            footer.DrawDividerLine = true;
                            footer.Height = 25;
                            footer.FontSize = 250;
                            footer.HtmlFragment = "<p>National Crisis/Suicide Prevention Hotline 800-273-8255:</p>" +
                                "<p>Crisis Text Line: Text HOME to 741741</p>";

                            qrDoc.AddHTMLFooters(footer);
                            page.AppendPdf(qrDoc);
                            
                            byte[] bytes = page.BinaryData;
                            student.IdPic = bytes;
                        }
                        else
                        {
                            byte[] bytes = page.BinaryData;
                            student.IdPic = bytes;
                        }
                        _schoolContext.SaveChanges();
                    }
                }
            }
            //string text = pdf.ExtractAllText();
            //string[] splitText = text.Split(new string[] { ",", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //RedirectToAction("ViewStudents");
            */
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            using (PdfDocument document = PdfDocument.Open(path))
            {
                
                foreach (Page page in document.GetPages())
                {
                    
                    //string pageText = page.Text;
                    //_logger.LogInformation(pageText);
                    var words = page.GetWords();
                    _logger.LogInformation("Name:" + words.ElementAt(6));
                    _logger.LogInformation("ID:" + words.ElementAt(8));
                    PdfDocumentBuilder builder = new PdfDocumentBuilder();
                    var firstPage = builder.AddPage(PageSize.A7);
                    
                    

                }
            }
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
