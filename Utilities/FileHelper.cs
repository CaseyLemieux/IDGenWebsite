﻿using BarcodeLib;
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
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Writer;

namespace IDGenWebsite.Utilities
{
    public class FileHelper
    {
        private readonly SchoolContext _schoolContext;
        //private readonly Iconverter _converter;
        public FileHelper(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
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
            await ParseQrCodeFilesAsync(filePaths.ElementAt(0));
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
                var id = Path.GetFileNameWithoutExtension(paths.ElementAt(0));
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

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        }

        private async Task ParseQrCodeFilesAsync(string fileName)
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
                        if (student != null)
                        {
                            student.DisplayName = reader.GetValue(3).ToString();
                            student.QrCode = reader.GetValue(4).ToString();
                        }
                    }
                    _schoolContext.SaveChanges();
                }
            }
        }

        public async Task GenerateId(int id, string templateRootPath)
        {
            var student = await _schoolContext.Students.SingleOrDefaultAsync(s => s.ID == id);
            if (student != null || student.IdPicPath != null)
            {
                Bitmap qrCodeImage = null;
                if(student.QrCode != null)
                {
                    //Generate QrCode BitMap
                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(student.QrCode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qRCode = new QRCode(qRCodeData);
                    qrCodeImage = qRCode.GetGraphic(15);
                }
                //Convert the bitmap to a byte array
                ImageConverter converter = new ImageConverter();
                byte[] qrCodeBytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                //Generate the Front Barcode
                Barcode barcode = new Barcode(student.StudentID, TYPE.CODE128);
                byte[] barcodeBytes = barcode.GetImageData(SaveTypes.JPG);

                //Convert the Barcode and QrCode to Base64 strings
                string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                string barcodeBase64 = Convert.ToBase64String(barcodeBytes);

                //Get the front and back templates
                string frontTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateFront.html"));
                string backTemplate = File.ReadAllText(Path.Combine(templateRootPath, "IdTemplateBack.html"));

                //Replace the place holder strings on the front template
                frontTemplate = frontTemplate.Replace("[PATH]", templateRootPath)
                    .Replace("[SCHOOL]", "Elementary")
                    .Replace("[IDPHOTO]", student.IdPicPath)
                    .Replace("[NAME]", student.DisplayName)
                    .Replace("[BARCODE]", barcodeBase64);
                //Replace the place holder strings on the back template
                backTemplate = backTemplate.Replace("[QRCODE]", qrCodeBase64);
            }
        }

    }
}