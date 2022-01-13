
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
using System.IO.Compression;
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
        private string qrHTML;
        private string webroot;
        private string fillerHtml;
        
        public FileHelper(SchoolContext schoolContext, IConverter converter, string webroot)
        {
            _schoolContext = schoolContext;
            _converter = converter;
            this.webroot = webroot;
            qrHTML = 
                @"<div class=""col - 3"">
                  <div class=""back"">
                    <h1 class=""Details"">Classlink Qr Code</h1>
                    <div class=""back-middle"">
                        <img src = ""data:image/png;base64, [QRCODE]"">
                    </div>
                    <div class=""bottom-back"">               
                        <p>[NAME]</p>
                        <p>[EMAIL]</p>
                        <p>[IDNUMBER]</p>
                    </div>
                  </div>
                </div>";
            fillerHtml =
                 @"<div class=""col - 3"">
                </div>";
        }
        public FileHelper(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }
        public async Task UploadIdsAsync(List<IFormFile> idFiles)
        {
            var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Id Pictures/", idFiles);
            await ParseIdFilesAsync(filePaths);

        }

        //Used for Manual Upload of Student Data by Excel Sheet
        public async Task UploadStudentsFileAsync(List<IFormFile> studentFiles)
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

        //TODO: Change how upload writes over previous student image
        private async Task ParseIdFilesAsync(List<string> paths)
        {
            foreach (var path in paths)
            {
                var id = Path.GetFileNameWithoutExtension(path);
                var student = await _schoolContext.Users.SingleOrDefaultAsync(s => s.Identifier == id);
                if (student != null)
                {
                    student.IdPicPath = path;
                }
                _schoolContext.SaveChanges();
            }
        }

        private async Task ParseStudentFilesAsync(string fileName)
        {

            //HashSet<string> homerooms = new HashSet<string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                  
                    while (reader.Read())
                    {
                        Users student = new Users();
                        student.Identifier = reader.GetValue(0).ToString();
                        student.FamilyName = reader.GetValue(1).ToString();
                        student.GivenName = reader.GetValue(2).ToString();
                        student.Email = reader.GetValue(3).ToString();
                        student.Grades.Add(new Grades
                        {
                            Description = reader.GetValue(4).ToString()
                        });
                        /*student.EnrollmentStartDate = DateTime.Parse(reader.GetValue(5).ToString());
                        student.HomeRoomTeacher = reader.GetValue(6).ToString().Trim();
                        student.HomeRoomTeacherEmail = reader.GetValue(7).ToString(); */

                        //homerooms.Add(reader.GetValue(6).ToString().Trim());
                      
                       

                        var dbEntry = await _schoolContext.Users.FirstOrDefaultAsync(s => s.Identifier == student.Identifier);
                        if (dbEntry == null)
                        {
                            _schoolContext.Add(student);
                        }
                      
                    }
                    _schoolContext.SaveChanges();
                }
            }
            /*foreach(string room in homerooms)
            {
                HomeroomsModel homeroomsModel = new HomeroomsModel()
                {
                    Teacher = room
                };
                _schoolContext.Homerooms.Add(homeroomsModel);
            } */
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
                                var student = await _schoolContext.Users.SingleOrDefaultAsync(s => s.Email == reader.GetValue(2).ToString());
                                if (student != null)
                                {
                                    //student.DisplayName = reader.GetValue(3).ToString();
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

        public byte[] GenerateId(Users student)
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
                Image img = barcode.Encode(TYPE.CODE128, student.Identifier, Color.Black, Color.White, 200, 35);
                Bitmap barcodeBitmap = (Bitmap)img;
                byte[] barcodeBytes = (byte[])converter.ConvertTo(barcodeBitmap, typeof(byte[]));

                //Convert the Barcode and QrCode to Base64 strings
                string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                string barcodeBase64 = Convert.ToBase64String(barcodeBytes);

                //Convert Logo and Pic To Base64
                string idPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(student.IdPicPath));
                string logoPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(webroot + "/Images/FCSD_Hawk.png"));
                string template = null;
                string school = null;
                switch (student.Grades.ElementAt(0).Code)
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
                string frontTemplate = File.ReadAllText(Path.Combine(webroot, "IdTemplateFront.html"));
                string backTemplate = File.ReadAllText(Path.Combine(webroot, "IdTemplateBack.html"));

                //Replace the place holder strings on the front template
                frontTemplate = frontTemplate.Replace("[PATH]", webroot)
                    .Replace("[STYLESHEET]", template)
                    .Replace("[LOGO]", logoPhotoBase64)
                    .Replace("[SCHOOL]", school)
                    .Replace("[IDPHOTO]", idPhotoBase64)
                    .Replace("[NAME]", student.GivenName + " " +student.FamilyName)
                    .Replace("[GRADE]", student.Grades.ElementAt(0).Code)
                    .Replace("[BARCODE]", barcodeBase64)
                    .Replace("[IDNUMBER]", student.Identifier);
                //Replace the place holder strings on the back template
                backTemplate = backTemplate.Replace("[QRCODE]", qrCodeBase64)
                    .Replace("[STYLESHEET]", template);

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
                         WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(webroot, "css", template) }
                    },
                     new ObjectSettings()
                    {
                        HtmlContent = backTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(webroot, "css", template) }

                    }
                }
                };

               
                byte[] pdf = _converter.Convert(doc);
                return pdf;
            }

            //for now return null
            return null;
        }

        public byte[] GenerateQrCode(Users student)
        {
            //IF the student and qr isnt null for somereason generate the qr code
            if (student != null && student.QrCode != null)
            {
                //Generate QrCode BitMap
                Bitmap qrCodeImage = null;
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);
                qrCodeImage = qRCode.GetGraphic(1);

                //Convert the bitmap to a byte array
                ImageConverter converter = new ImageConverter();
                byte[] qrCodeBytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                //Convert the QrCode to Base64 string
                string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                //Get the Qr Code Template
                string qrTemplate = File.ReadAllText(Path.Combine(webroot, "QrCodeTemplate.html"));

                string html = qrTemplate.Replace("[QRCODE]", qrCodeBase64)
                           .Replace("[NAME]", student.GivenName + " " + student.FamilyName)
                           .Replace("[EMAIL]", student.Email)
                           .Replace("[IDNUMBER]", student.Identifier)
                           .Replace("[PATH]", webroot);

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
                         HtmlContent = html,
                         WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(webroot, "css", "/css/QrCodeTemplateStyleSheet.css"), EnableJavascript = true, enablePlugins = true, PrintMediaType = true },
                         LoadSettings = { BlockLocalFileAccess = false }
                    }

                    }
                };

                byte[] pdf = _converter.Convert(doc);
                return pdf;

            }

            //Return null if something goes wrong
            return null;
        }

        public byte[] GenerateQrCodes(List<Users> students)
        {
            Dictionary<Users, string> studentQrCodes = new Dictionary<Users, string>();
            foreach(Users student in students)
            {
                if (student != null && student.QrCode != null)
                {
                    //Generate QrCode BitMap
                    Bitmap qrCodeImage;
                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qRCode = new QRCode(qRCodeData);
                    qrCodeImage = qRCode.GetGraphic(1);

                    //Convert the bitmap to a byte array
                    ImageConverter converter = new ImageConverter();
                    byte[] qrCodeBytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                    //Convert the QrCode to Base64 string
                    string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                    studentQrCodes.Add(student, qrCodeBase64);
                }
            }

            if(studentQrCodes.Count > 0)
            {
                //Get the Qr Code Template
                string qrTemplate = File.ReadAllText(Path.Combine(webroot, "QrCodeSheetTemplate.html"));

                //Get the number of rows. If even division thats the number if not, add one to get the correct amount. 
                int numberOfRows = studentQrCodes.Count / 3 + (studentQrCodes.Count % 3 > 0 ? 1 : 0);
                int currentStudentNumber = 0;
                string bodyHtml = "";
                List<string> rows = new List<string>();
                //Iterate through the rows
                //Will currently thow erros because it does not take into account whether it has actually ran out of students on the last row when it rounds up. 
                for (int i = 0; i < numberOfRows; i++)
                {
                    bodyHtml = string.Concat(bodyHtml, @"<div class=""row"">");
                    //Iteratre through the three columns
                    for (int j = 0; j < 3 && currentStudentNumber < studentQrCodes.Count; j++)
                    {
                        var currentStudent = studentQrCodes.ElementAt(currentStudentNumber);
                        string html = qrHTML.Replace("[QRCODE]", currentStudent.Value)
                            .Replace("[NAME]", currentStudent.Key.GivenName + " " + currentStudent.Key.FamilyName)
                            .Replace("[EMAIL]", currentStudent.Key.Email)
                            .Replace("[IDNUMBER]", currentStudent.Key.Identifier);
                        bodyHtml = string.Concat(bodyHtml, html);
                        currentStudentNumber++;

                    }
                    if (i == numberOfRows - 1)
                    {
                        //Calculate how many filler columns to add based on whether there are left over spots
                        int maxIDs = numberOfRows * 3;
                        int leftOverSpots = maxIDs - studentQrCodes.Count;
                        for (int k = 0; k < leftOverSpots; k++)
                        {
                            bodyHtml = string.Concat(bodyHtml, fillerHtml);
                        }
                    }
                    bodyHtml = string.Concat(bodyHtml, @"</div>");
                    rows.Add(bodyHtml);
                    bodyHtml = "";
                }

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings =
                {
                    PaperSize = PaperKind.Letter,
                    Orientation = Orientation.Portrait,

                },
                };

                for (int i = 0, skip = 0; i < rows.Count; i++, skip += 3)
                {
                    var pageRows = rows.Skip(skip).Take(3);
                    var htmlContent = "";
                    foreach (string row in pageRows)
                    {
                        htmlContent = string.Concat(htmlContent, row);
                    }
                    htmlContent = qrTemplate.Replace("[PATH]", webroot).Replace("[BODY]", htmlContent);
                    ObjectSettings page = new ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(webroot, "css", "/css/QrCodeSheetTemplateStyleSheet.css"), EnableJavascript = true, enablePlugins = true, PrintMediaType = true },
                        LoadSettings = { BlockLocalFileAccess = false }
                    };
                    doc.Objects.Add(page);
                }

                byte[] pdf = _converter.Convert(doc);
                return pdf;
            }

            return null;
        }

        public byte[] GenerateZipFile(List<ZipItem> zipItems)
        {
            byte[] bytes;
            using (var zipStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var zipItem in zipItems)
                    {
                        var entry = zip.CreateEntry(zipItem.Name + ".pdf", CompressionLevel.Optimal);
                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(zipItem.Content, 0, zipItem.Content.Length);
                        }
                    }
                }
                bytes = zipStream.ToArray();
            }
            return bytes;
        }

        public byte[] GenerateZipFile(Dictionary<string, List<ZipItem>> teacherList)
        {
            byte[] bytes;
            using (var zipStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach(KeyValuePair<string, List<ZipItem>> entry in teacherList)
                    {
                        foreach (var zipItem in entry.Value)
                        {
                            var zipEntry = zip.CreateEntry(string.Concat(entry.Key, "/", zipItem.Name, ".pdf"), CompressionLevel.Optimal);
                            using (var entryStream = zipEntry.Open())
                            {
                                entryStream.Write(zipItem.Content, 0, zipItem.Content.Length);
                            }
                        }
                    }
                    
                }
                bytes = zipStream.ToArray();
            }
            return bytes;
        }
    }
}
