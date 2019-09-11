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

        private readonly string[] _operationTypeTitles;

        public HomeController(
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

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

        public IActionResult Gallery()
        {
            var targetOperationTitles = new string[]
            {
                _operationTypeTitles[0],
                _operationTypeTitles[9],
                _operationTypeTitles[14],
                _operationTypeTitles[15]
            };

            var operationsWebRootPath = _hostingEnvironment.WebRootPath + "\\images\\operations-multitypes";
            var operationDirectories = Directory.GetDirectories(operationsWebRootPath);
            var directoryOperationTitleMap = operationDirectories
                .Zip(targetOperationTitles, (path, title) => new { path, title })
                .ToDictionary(x => x.title, x => x.path);

            var result = new List<OperationTypeViewModel>();
            foreach (var item in directoryOperationTitleMap)
            {
                var sampleDirectories = Directory.GetDirectories(item.Value);
                var samplesDictionary = new List<Dictionary<string, string>>();

                foreach (var sampleDirectory in sampleDirectories)
                {
                    var sampleFiles = Directory.GetFiles(sampleDirectory)
                        .Select(x => x.Replace(_hostingEnvironment.WebRootPath, ""));
                    var sampleThumbnails = Directory.GetFiles(sampleDirectory + "\\thumbnails")
                        .Select(x => x.Replace(_hostingEnvironment.WebRootPath, ""));

                    var sampleDictionary = sampleFiles
                        .Zip(sampleThumbnails, (image, thumbnail) => new { image, thumbnail })
                        .ToDictionary(x => x.image, x => x.thumbnail);

                    samplesDictionary.Add(sampleDictionary);
                }

                result.Add(new OperationTypeViewModel()
                {
                    Title = item.Key,
                    Samples = samplesDictionary
                });
            }

            var viewModel = new GalleryViewModel()
            {
                OperationTypes = result
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
            var certificatesWebRootPath = _hostingEnvironment.WebRootPath + "\\images\\certificates";
            var certificateThumbnailsWebRootPath = _hostingEnvironment.WebRootPath + "\\images\\certificates\\thumbnails";
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
