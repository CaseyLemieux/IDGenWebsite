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
        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=idgenblob;AccountKey=JZ2erPcBSYeVbYB6DBnOWoy9MKt1WU0ENFKeFdE/OmsFnJFVY8Aq1rJDQfxg87GBdZY/qnZG8AjC3oF74v0uZg==;EndpointSuffix=core.windows.net";
        //private readonly Iconverter _converter;
        public FileHelper(SchoolContext schoolContext, IConverter converter)
        {
            _schoolContext = schoolContext;
            _converter = converter;
        }
        public async Task UploadIdsAsync(List<IFormFile> idFiles)
        {
            //var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Id Pictures/", idFiles);
            var filePaths = await SaveIdFilesToBlob(idFiles);
            await ParseIdFilesAsync(filePaths);
            
        }

        public async Task UploadStudentsAsync(List<IFormFile> studentFiles)
        {
            //var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Student Imports/", studentFiles);
            var filePaths = await SaveFilesToBlob(studentFiles);
            await ParseStudentFilesAsync(filePaths.ElementAt(0));
        }

        public async Task UploadQrCodesAsync(List<IFormFile> qrCodeFiles)
        {
            //var filePaths = await SaveFiles("C:/IDGenWebsite/Uploads/Qr Code Imports/", qrCodeFiles);
            var filePaths = await SaveFilesToBlob(qrCodeFiles);
            await ParseQrCodeFilesAsync(filePaths.ElementAt(0));
        }

        private async Task<List<string>> SaveFilesToBlob(List<IFormFile> files)
        {
            List<string> paths = new List<string>();

            BlobServiceClient blobClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "studentimports");
            foreach (var formFile in files)
            {
                try
                {
                    StreamReader stream = new StreamReader(formFile.OpenReadStream());
                    string fileName = Path.GetFileName(formFile.FileName);
                    await containerClient.UploadBlobAsync(fileName, stream.BaseStream);
                    paths.Add(fileName);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return paths;
        }

        private async Task<List<string>> SaveIdFilesToBlob(List<IFormFile> files)
        {
            List<string> paths = new List<string>();
            
            BlobServiceClient blobClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "idpictures");
            foreach(var formFile in files)
            {
                try
                {
                    StreamReader stream = new StreamReader(formFile.OpenReadStream());
                    string fileName = Path.GetFileName(formFile.FileName);
                    await containerClient.UploadBlobAsync(fileName, stream.BaseStream);
                    paths.Add(fileName);
                } catch(Exception ex)
                {
                    throw;
                }
            }
            return paths;
        }

        /*private async Task<List<string>> SaveFiles(string directoryPath, List<IFormFile> files)
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
        } */

        private async Task ParseIdFilesAsync(List<string> paths)
        {
            //throw new Exception(name);
            foreach (var path in paths)
            {
                var id = Path.GetFileNameWithoutExtension(path);
                var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.StudentID == id);
                if(student != null)
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

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "studentimports");
            var blob = containerClient.GetBlobClient(fileName);
            var blobDownload = await blob.DownloadContentAsync();
            

            /*using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                
            } */
            using (var reader = ExcelReaderFactory.CreateReader(blobDownload.Value.Content.ToStream()))
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
                    student.HomeRoomTeacher = reader.GetValue(6).ToString();
                    student.HomeRoomTeacherEmail = reader.GetValue(7).ToString();

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

        private async Task ParseQrCodeFilesAsync(string fileName)
        {
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "studentimports");
            var blob = containerClient.GetBlobClient(fileName);
            var blobDownload = await blob.DownloadContentAsync();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateReader(blobDownload.Value.Content.ToStream()))
            {
                reader.Read();
                while (reader.Read())
                {
                    var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.Email == reader.GetValue(2).ToString());
                    if (student != null)
                    {
                        student.DisplayName = reader.GetValue(3).ToString();
                        student.QrCode = reader.GetValue(4).ToString();
                    }
                }
                _schoolContext.SaveChanges();
            }
        }

        public byte[] GenerateId(StudentModel student, string templateRootPath)
        {

            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "idpictures");
            

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
                Image img = barcode.Encode(TYPE.CODE128, student.StudentID, Color.Black, Color.White, 200, 20);
                Bitmap barcodeBitmap = (Bitmap)img;
                byte[] barcodeBytes = (byte[])converter.ConvertTo(barcodeBitmap, typeof(byte[]));

                //Convert the Barcode and QrCode to Base64 strings
                string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                string barcodeBase64 = Convert.ToBase64String(barcodeBytes);

                //Convert Logo and Pic To Base64
                //Get Student ID from blob
                var blob = containerClient.GetBlobClient(student.IdPicPath);
                var blobDownload = blob.DownloadContent();
                string idPhotoBase64 = Convert.ToBase64String(blobDownload.Value.Content.ToArray());
                string logoPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(templateRootPath + "/Images/FCSD_Hawk.png"));

                //Get the front and back templates
                string frontTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateFront.html"));
                string backTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateBack.html"));

                //Replace the place holder strings on the front template
                frontTemplate = frontTemplate.Replace("[PATH]", templateRootPath)
                    .Replace("[LOGO]", logoPhotoBase64)
                    .Replace("[SCHOOL]", "Elementary")
                    .Replace("[IDPHOTO]", idPhotoBase64)
                    .Replace("[NAME]", student.DisplayName)
                    .Replace("[GRADE]", student.GradeLevel)
                    .Replace("[BARCODE]", barcodeBase64);
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
                         WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", "IdTemplateStyleSheet.css") }
                    },
                     new ObjectSettings()
                    {
                        HtmlContent = backTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(templateRootPath, "css", "IdTemplateStyleSheet.css") }

                    }
                }
                };

                byte[] pdf = _converter.Convert(doc);

                return pdf;
            }

            //for now return null
            return null;
        }


    }
}
