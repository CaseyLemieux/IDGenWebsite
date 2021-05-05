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
using IronPdf;
using QRCoder;
using System.Drawing;

namespace IDGenWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SchoolContext _context;
        //private readonly IWebHostEnvironment _env;

        public AdminController(ILogger<AdminController> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
        //[HttpGet]
        public async Task<IActionResult> ViewStudents()
        {
            var user = User.Identity.Name;
            return View(await _context.Students.ToListAsync());
        }

        public async Task<IActionResult> ParseClasslinkFile(string fileName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        var student = await _context.Students.SingleOrDefaultAsync(s => s.Email == reader.GetValue(2).ToString());
                        if(student != null)
                        {
                            student.DisplayName = reader.GetValue(3).ToString();
                            student.QrCode = reader.GetValue(4).ToString();
                        }
                    }
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("ViewStudents");
        }

        public async Task<IActionResult> TestExcel()
        {
            //List<StudentModel> students = new List<StudentModel>();

            var fileName = "./Focus Students Feburary.xlsx";

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

                        var dbEntry = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == student.StudentID);
                        if(dbEntry == null)
                        {
                             _context.Add(student);
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
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("ViewStudents");
        }
        public IActionResult UploadStudents()
        {
            return View();
        }
        [HttpPost("FocusFileUpload")]
        public async Task<IActionResult> UploadFocus(List<IFormFile> focusFiles)
        {
            var folderName = "Uploads";

            if (focusFiles != null)
            {
                foreach (var formFile in focusFiles)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory());
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using(var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //var fileName = filePaths.FirstOrDefault();
            return RedirectToAction("TestExcel");
        }

        [HttpPost("ClassLinkFileUpload")]
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
            return await ParseClasslinkFile(fullPath);
        }

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
            return await ParseFocusPDF(fullPath);
        }

        public async Task<IActionResult> ParseFocusPDF(string path)
        {
          
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
                    var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
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
                            page.AppendPdf(qrDoc);
                            byte[] bytes = page.BinaryData;
                            student.IdPic = bytes;
                        }
                        else
                        {
                            byte[] bytes = page.BinaryData;
                            student.IdPic = bytes;
                        }
                        _context.SaveChanges();
                    }
                }
            }
            //string text = pdf.ExtractAllText();
            //string[] splitText = text.Split(new string[] { ",", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return RedirectToAction("ViewStudents");
        }
        [HttpPost]
        public async Task<IActionResult> SaveID(int id)
        {
            
            var student = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            if (student != null && student.IdPic != null)
            {
                return File(student.IdPic, "application/pdf", string.Concat(student.Email, ".pdf"));
            }

            return RedirectToAction("ViewStudents");
        }
    }
}
