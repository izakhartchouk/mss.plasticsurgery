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

        private readonly string[] _operationTypeTitles;

        public HomeController(
            IHostingEnvironment hostingEnvironment,
            IGenericRepository<Operation, int> operationRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _operationRepository = operationRepository;

            _operationTypeTitles = new string[]
            {
                "Увеличение груди (аугментационная маммопластика)",
                "Подтяжка груди (мастопексия)",
                "Уменьшение груди (редукционная маммопластика)",
                "Пластика век (блефаропластика)",
                "Пластика передней брюшной стенки (абдоминопластика)",
                "Пластика ушных раковин (отопластика)",
                "Увеличение голеней (круропластика)",
                "Уменьшение объёма жировой ткани (липосакция)",
                "Пересадка волос (HFE аутотрансплантация)",
                "Устранение гинекомастии (маскулинизирующая маммопластика)",
                "Введение жировой ткани (липофилинг)",
                "Протезирование полового члена (эндофаллопротезирование)",
                "Подтяжка нижней трети лица (фейслифтинг)",
                "Пластика носа (ринопластика)",
                "Удаление комочков Биша",
                "Маскулинизирующая маммопластика при транссексуализме",
                "Пластика груди после мастэктомии",
                "Пластика полового члена (фаллопластика)",
                "Пластика дефектов мягких тканей",
                "Хирургия кисти (удаление олеогранулёмы, устранение контрактуры Дюпюитрена)",
                "Микрохирургическая аутотрансплантация пальцев стопы на кисть"
            };
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

        public IActionResult Gallery()
        {
            var certificatesWebRootPath = _hostingEnvironment.WebRootPath + "\\img\\operations-multitypes\\1-breast-augmentation\\sample-1";
            var certificateThumbnailsWebRootPath = _hostingEnvironment.WebRootPath + "\\img\\operations-multitypes\\1-breast-augmentation\\sample-1\\thumbnails";
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

            var viewModel = new GalleryViewModel()
            {
                OperationTypeTitles = _operationTypeTitles,
                ImagesAndThumbnails = relativePaths
            };

            return View(viewModel);
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult BeforeOperation()
        {
            return View();
        }

        public IActionResult AfterOperation()
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

        public IActionResult OperationTypes()
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
