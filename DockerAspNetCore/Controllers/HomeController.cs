using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DockerAspNetCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;

namespace DockerAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProvider _fileProvider;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IFileProvider fileProvider, IConfiguration configuration)
        {
            _logger = logger;
            _fileProvider = fileProvider;
            _configuration = configuration;
        }

     
        public IActionResult Index()
        {
            ViewBag.SqlCon = _configuration["SqlCon"];
            return View();
        }

        public IActionResult SaveImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveImage(IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

            }

            return View();
        }

        public IActionResult ImageShow()
        {
            var images = _fileProvider.GetDirectoryContents("wwwroot/images").ToList().Select(x => x.Name);
            return View(images);
        }

        [HttpPost]
        public IActionResult ImageShow(string name)
        {
            var file = _fileProvider.GetDirectoryContents("wwwroot/images").ToList().First(x => x.Name == name);

            System.IO.File.Delete(file.PhysicalPath);

            return RedirectToAction("ImageShow");
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
