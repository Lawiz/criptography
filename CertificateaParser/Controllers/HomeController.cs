using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CertificateaParser.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace CertificateaParser.Controllers
{
    public class HomeController : Controller
    {

        private X509Certificate2 certificate2 = new X509Certificate2();

        [HttpPost]
        public async Task<dynamic> UploadFile(IFormFile file)
        {
            byte[] bytes = new byte[file.Length];
            using (var reader = file.OpenReadStream())
            {
                await reader.ReadAsync(bytes, 0, (int) file.Length);
            }

            certificate2 = new X509Certificate2(bytes);
            return View("Index", certificate2);
        }

        [HttpGet]
        public dynamic UploadFile()
        {
            return View("UploadFile");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(X509Certificate2 model)
        {
            var tempFile = CreateTmpFile();
            System.IO.File.WriteAllText(tempFile,ExportToPEM(model));
            return new PhysicalFileResult(tempFile,MediaTypeHeaderValue.Parse());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        

        private static string CreateTmpFile()
        {
            string fileName = string.Empty;

            try
            {
                
                fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pem";

                FileInfo fileInfo = new FileInfo(fileName);

               
                fileInfo.Attributes = FileAttributes.Temporary;
                

                Console.WriteLine("TEMP file created at: " + fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to create TEMP file or set its attributes: " + ex.Message);
            }

            return fileName;

        }
        public static string ExportToPEM(X509Certificate2 cert)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }
    }
}
