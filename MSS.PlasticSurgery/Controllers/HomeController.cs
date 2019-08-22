using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;
using MSS.PlasticSurgery.Models;

namespace MSS.PlasticSurgery.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGenericRepository<Operation, int> _operationRepository;

        public HomeController(
            IHostingEnvironment hostingEnvironment,
            IGenericRepository<Operation, int> operationRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _operationRepository = operationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult PhotoGallery()
        {
            var operationViewModels = _operationRepository.GetAll()
                .Select(x => new OperationViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    Description = x.Description,
                    Images = x.Images.Select(y => y.Path)
                });

            return View(operationViewModels);
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult OurClients()
        {
            return View();
        }

        public IActionResult OurStaff()
        {
            return View();
        }

        public IActionResult Certificates()
        {
            var certificatesWebRootPath = _hostingEnvironment.WebRootPath + "\\img\\certificates";
            var certificateThumbnailsWebRootPath = _hostingEnvironment.WebRootPath + "\\img\\certificates\\thumbnails";
            string[] filesArray = Directory.GetFiles(certificatesWebRootPath);
            string[] thumbnailsArray = Directory.GetFiles(certificateThumbnailsWebRootPath);

            var relativePaths = new Dictionary<string, string>();
            for (int i = 0; i < filesArray.Length; i++)
            {
                var filePath = filesArray[i];
                var thumbnailPath = thumbnailsArray[i];

                var relativeThumbnailImagePath = thumbnailPath
                    .Replace(_hostingEnvironment.WebRootPath, "")
                    .Replace("\\", "/");

                var relativeOriginalImagePath = filePath
                    .Replace(_hostingEnvironment.WebRootPath, "")
                    .Replace("\\", "/");

                relativePaths.Add(relativeOriginalImagePath, relativeThumbnailImagePath);
            }

            return View(relativePaths);
        }

        public IActionResult HotOffers()
        {
            return View();
        }

        public IActionResult Procedures()
        {
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
