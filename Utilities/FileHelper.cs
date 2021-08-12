using Azure.Storage.Blobs;
using BarcodeLib;
using ExcelDataReader;
using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace IDGenWebsite.Utilities
{
    public class FileHelper
    {
        private readonly SchoolContext _schoolContext;
        private readonly IConverter _converter;
        //private readonly Iconverter _converter;
        public FileHelper(SchoolContext schoolContext, IConverter converter)
        {
            _schoolContext = schoolContext;
            _converter = converter;
        }
        public async Task UploadIdsAsync(List<IFormFile> idFiles)
        {
            var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Id Pictures/", idFiles);
            await ParseIdFilesAsync(filePaths);

        }

        public async Task UploadStudentsAsync(List<IFormFile> studentFiles)
        {
            var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Student Imports/", studentFiles);
            await ParseStudentFilesAsync(filePaths.ElementAt(0));
        }

        public async Task UploadQrCodesAsync(List<IFormFile> qrCodeFiles)
        {
            var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Qr Code Imports/", qrCodeFiles);
            await ParseQrCodeFilesAsync(filePaths);
        }

        private async Task<List<string>> SaveFiles(string directoryPath, List<IFormFile> files)
        {
            List<string> paths = new List<string>();
            //TODO: Need to save a refrence to the file in the Database
            foreach (var formFile in files)
            {
                var fileName = Path.GetFileName(formFile.FileName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory());
                var fullPath = Path.Combine(directoryPath, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                paths.Add(fullPath);
            }
            return paths;
        }

        private async Task ParseIdFilesAsync(List<string> paths)
        {
            //throw new Exception(name);
            foreach (var path in paths)
            {
                var id = Path.GetFileNameWithoutExtension(path);
                var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.StudentID == id);
                if (student != null)
                {
                    student.IdPicPath = path;
                }
                _schoolContext.SaveChanges();
            }
        }

        private async Task ParseStudentFilesAsync(string fileName)
        {
            //List<StudentModel> students = new List<StudentModel>();

            //var fileName = "./Focus Students May.xlsx";
            HashSet<string> homerooms = new HashSet<string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
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
                        student.EnrollmentStartDate = DateTime.Parse(reader.GetValue(5).ToString());
                        student.HomeRoomTeacher = reader.GetValue(6).ToString().Trim();
                        student.HomeRoomTeacherEmail = reader.GetValue(7).ToString();

                        homerooms.Add(reader.GetValue(6).ToString().Trim());
                      
                       

                        var dbEntry = await _schoolContext.Students.FirstOrDefaultAsync(s => s.StudentID == student.StudentID);
                        if (dbEntry == null)
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
            foreach(string room in homerooms)
            {
                HomeroomsModel homeroomsModel = new HomeroomsModel()
                {
                    Teacher = room
                };
                _schoolContext.Homerooms.Add(homeroomsModel);
            }
            _schoolContext.SaveChanges();
        }

        private async Task ParseQrCodeFilesAsync(List<string> paths)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            foreach(string path in paths)
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            try
                            {
                                var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.Email == reader.GetValue(2).ToString());
                                if (student != null)
                                {
                                    student.DisplayName = reader.GetValue(3).ToString();
                                    student.QrCode = reader.GetValue(4).ToString();
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                Debug.WriteLine(ex);
                                Debug.WriteLine("Student Email:" + reader.GetValue(2).ToString());
                            }

                        }
                        _schoolContext.SaveChanges();
                    }
                }
            }
        }

        public byte[] GenerateId(StudentModel student, string templateRootPath)
        {

            if (student != null && student.IdPicPath != null && student.QrCode != null)
            {
                //Generate QrCode BitMap
                Bitmap qrCodeImage = null;
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);
                qrCodeImage = qRCode.GetGraphic(15);

                //Convert the bitmap to a byte array
                ImageConverter converter = new ImageConverter();
                byte[] qrCodeBytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                //Generate the Front Barcode
                Barcode barcode = new Barcode();
                Image img = barcode.Encode(TYPE.CODE128, student.StudentID, Color.Black, Color.White, 200, 35);
                Bitmap barcodeBitmap = (Bitmap)img;
                byte[] barcodeBytes = (byte[])converter.ConvertTo(barcodeBitmap, typeof(byte[]));

                //Convert the Barcode and QrCode to Base64 strings
                string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                string barcodeBase64 = Convert.ToBase64String(barcodeBytes);

                //Convert Logo and Pic To Base64
                string idPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(student.IdPicPath));
                string logoPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(templateRootPath + "/Images/FCSD_Hawk.png"));
                string template = null;
                string school = null;
                switch (student.GradeLevel)
                {
                    case "PK":
                        template = "ElementaryStyleSheet.css";
                        school = "Learning Center";
                        break;
                    case "KG":
                        template = "ElementaryStyleSheet.css";
                        school = "Elementary";
                        break;
                    case "01":
                        template = "ElementaryStyleSheet.css";
                        school = "Elementary";
                        break;
                    case "02":
                        template = "ElementaryStyleSheet.css";
                        school = "Elementary";
                        break;
                    case "03":
                        template = "ElementaryStyleSheet.css";
                        school = "Elementary";
                        break;
                    case "04":
                        template = "ElementaryStyleSheet.css";
                        school = "Elementary";
                        break;
                    case "05":
                        template = "ElementaryStyleSheet.css";
                        school = "Elementary";
                        break;
                    case "06":
                        template = "MiddleStyleSheet.css";
                        school = "Middle";
                        break;
                    case "07":
                        template = "MiddleStyleSheet.css";
                        school = "Middle";
                        break;
                    case "08":
                        template = "MiddleStyleSheet.css";
                        school = "Middle";
                        break;
                    case "09":
                        template = "HighStyleSheet.css";
                        school = "High";
                        break;
                    case "10":
                        template = "HighStyleSheet.css";
                        school = "High";
                        break;
                    case "11":
                        template = "HighStyleSheet.css";
                        school = "High";
                        break;
                    case "12":
                        template = "HighStyleSheet.css";
                        school = "High";
                        break;
                }
                //Get the front and back templates
                string frontTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateFront.html"));
                string backTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateBack.html"));

                //Replace the place holder strings on the front template
                frontTemplate = frontTemplate.Replace("[PATH]", templateRootPath)
                    .Replace("[LOGO]", logoPhotoBase64)
                    .Replace("[SCHOOL]", school)
                    .Replace("[IDPHOTO]", idPhotoBase64)
                    .Replace("[NAME]", student.FirstName + " " +student.LastName)
                    .Replace("[GRADE]", student.GradeLevel)
                    .Replace("[BARCODE]", barcodeBase64)
                    .Replace("[IDNUMBER]", student.StudentID);
                //Replace the place holder strings on the back template
                backTemplate = backTemplate.Replace("[QRCODE]", qrCodeBase64);

                var doc = new HtmlToPdfDocument()
                {

                    GlobalSettings = {
                    PaperSize = new PechkinPaperSize("53mm", "84mm"),
                    ImageDPI = 300,
                    Margins = new MarginSettings(0, 0, 0, 0),
                    Orientation = Orientation.Portrait,
                },

                    Objects = {
                    new ObjectSettings()
                    {
                         HtmlContent = frontTemplate,
                         WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", template) }
                    },
                     new ObjectSettings()
                    {
                        HtmlContent = backTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", template) }

                    }
                }
                };

                
                
                byte[] pdf = _converter.Convert(doc);

                return pdf;
            }

            //for now return null
            return null;
        }

        public byte[] GenerateGradeLevel(List<StudentModel> students, string templateRootPath)
        {
            List<ObjectSettings> studentIds = new List<ObjectSettings>();
            foreach (StudentModel student in students)
            {
                if (student != null && student.IdPicPath != null && student.QrCode != null)
                {
                    //Generate QrCode BitMap
                    Bitmap qrCodeImage = null;
                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qRCode = new QRCode(qRCodeData);
                    qrCodeImage = qRCode.GetGraphic(15);

                    //Convert the bitmap to a byte array
                    ImageConverter converter = new ImageConverter();
                    byte[] qrCodeBytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                    //Generate the Front Barcode
                    Barcode barcode = new Barcode();
                    Image img = barcode.Encode(TYPE.CODE128, student.StudentID, Color.Black, Color.White, 200, 35);
                    Bitmap barcodeBitmap = (Bitmap)img;
                    byte[] barcodeBytes = (byte[])converter.ConvertTo(barcodeBitmap, typeof(byte[]));

                    //Convert the Barcode and QrCode to Base64 strings
                    string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                    string barcodeBase64 = Convert.ToBase64String(barcodeBytes);

                    //Convert Logo and Pic To Base64
                    string idPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(student.IdPicPath));
                    string logoPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(templateRootPath + "/Images/FCSD_Hawk.png"));
                    string template = null;
                    string school = null;
                    switch (student.GradeLevel)
                    {
                        case "PK":
                            template = "ElementaryStyleSheet.css";
                            school = "Learning Center";
                            break;
                        case "KG":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "01":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "02":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "03":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "04":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "05":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "06":
                            template = "MiddleStyleSheet.css";
                            school = "Middle";
                            break;
                        case "07":
                            template = "MiddleStyleSheet.css";
                            school = "Middle";
                            break;
                        case "08":
                            template = "MiddleStyleSheet.css";
                            school = "Middle";
                            break;
                        case "09":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                        case "10":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                        case "11":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                        case "12":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                    }
                    //Get the front and back templates
                    string frontTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateFront.html"));
                    string backTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateBack.html"));

                    //Replace the place holder strings on the front template
                    frontTemplate = frontTemplate.Replace("[PATH]", templateRootPath)
                        .Replace("[LOGO]", logoPhotoBase64)
                        .Replace("[SCHOOL]", school)
                        .Replace("[IDPHOTO]", idPhotoBase64)
                        .Replace("[NAME]", student.FirstName + " " + student.LastName)
                        .Replace("[GRADE]", student.GradeLevel)
                        .Replace("[BARCODE]", barcodeBase64)
                        .Replace("[IDNUMBER]", student.StudentID);
                    //Replace the place holder strings on the back template
                    backTemplate = backTemplate.Replace("[QRCODE]", qrCodeBase64);

                    ObjectSettings frontID = new ObjectSettings
                    {
                        HtmlContent = frontTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", template) }
                    };

                    ObjectSettings backID = new ObjectSettings
                    {
                        HtmlContent = backTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", template) }
                    };
                    studentIds.Add(frontID);
                    studentIds.Add(backID);
                }
            }

            if(studentIds.Count > 0)
            {
                var doc = new HtmlToPdfDocument()
                {

                    GlobalSettings =
                {
                    PaperSize = new PechkinPaperSize("53mm", "84mm"),
                    ImageDPI = 300,
                    Margins = new MarginSettings(0, 0, 0, 0),
                    Orientation = Orientation.Portrait,
                },
                    Objects = studentIds
                };
                byte[] pdf = _converter.Convert(doc);

                return pdf;
            }
            return null;
        }

        public byte[] GenerateHomeroom(List<StudentModel> students, string templateRootPath)
        {
            List<ObjectSettings> studentIds = new List<ObjectSettings>();
            foreach (StudentModel student in students)
            {
                if (student != null && student.IdPicPath != null && student.QrCode != null)
                {
                    //Generate QrCode BitMap
                    Bitmap qrCodeImage = null;
                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qRCode = new QRCode(qRCodeData);
                    qrCodeImage = qRCode.GetGraphic(15);

                    //Convert the bitmap to a byte array
                    ImageConverter converter = new ImageConverter();
                    byte[] qrCodeBytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                    //Generate the Front Barcode
                    Barcode barcode = new Barcode();
                    Image img = barcode.Encode(TYPE.CODE128, student.StudentID, Color.Black, Color.White, 200, 35);
                    Bitmap barcodeBitmap = (Bitmap)img;
                    byte[] barcodeBytes = (byte[])converter.ConvertTo(barcodeBitmap, typeof(byte[]));

                    //Convert the Barcode and QrCode to Base64 strings
                    string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                    string barcodeBase64 = Convert.ToBase64String(barcodeBytes);

                    //Convert Logo and Pic To Base64
                    string idPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(student.IdPicPath));
                    string logoPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(templateRootPath + "/Images/FCSD_Hawk.png"));
                    string template = null;
                    string school = null;
                    switch (student.GradeLevel)
                    {
                        case "PK":
                            template = "ElementaryStyleSheet.css";
                            school = "Learning Center";
                            break;
                        case "KG":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "01":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "02":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "03":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "04":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "05":
                            template = "ElementaryStyleSheet.css";
                            school = "Elementary";
                            break;
                        case "06":
                            template = "MiddleStyleSheet.css";
                            school = "Middle";
                            break;
                        case "07":
                            template = "MiddleStyleSheet.css";
                            school = "Middle";
                            break;
                        case "08":
                            template = "MiddleStyleSheet.css";
                            school = "Middle";
                            break;
                        case "09":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                        case "10":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                        case "11":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                        case "12":
                            template = "HighStyleSheet.css";
                            school = "High";
                            break;
                    }
                    //Get the front and back templates
                    string frontTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateFront.html"));
                    string backTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateBack.html"));

                    //Replace the place holder strings on the front template
                    frontTemplate = frontTemplate.Replace("[PATH]", templateRootPath)
                        .Replace("[LOGO]", logoPhotoBase64)
                        .Replace("[SCHOOL]", school)
                        .Replace("[IDPHOTO]", idPhotoBase64)
                        .Replace("[NAME]", student.FirstName + " " + student.LastName)
                        .Replace("[GRADE]", student.GradeLevel)
                        .Replace("[BARCODE]", barcodeBase64)
                        .Replace("[IDNUMBER]", student.StudentID);
                    //Replace the place holder strings on the back template
                    backTemplate = backTemplate.Replace("[QRCODE]", qrCodeBase64);

                    ObjectSettings frontID = new ObjectSettings
                    {
                        HtmlContent = frontTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", template) }
                    };

                    ObjectSettings backID = new ObjectSettings
                    {
                        HtmlContent = backTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", template) }
                    };
                    studentIds.Add(frontID);
                    studentIds.Add(backID);
                }
            }

            if(studentIds.Count > 0)
            {
                var doc = new HtmlToPdfDocument()
                {

                    GlobalSettings =
                {
                    PaperSize = new PechkinPaperSize("53mm", "84mm"),
                    ImageDPI = 300,
                    Margins = new MarginSettings(0, 0, 0, 0),
                    Orientation = Orientation.Portrait,
                },
                    Objects = studentIds
                };
                byte[] pdf = _converter.Convert(doc);

                return pdf;
            }
            return null;
        }
    }
}
