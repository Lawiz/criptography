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
                await reader.ReadAsync(bytes, 0, (int)file.Length);
            }
            certificate2 = new X509Certificate2(bytes);
            return View("Index",certificate2);
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
            return View("Index");
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
