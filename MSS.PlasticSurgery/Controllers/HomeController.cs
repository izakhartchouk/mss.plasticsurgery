using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private readonly List<KeyValuePair<int, string>> _operationTypeTitles;

        private const string FileExtensionsRegex = ".jpg|.png|.bmp";

        public HomeController(
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            _operationTypeTitles = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "Увеличение груди (аугментационная маммопластика)"),
                new KeyValuePair<int, string>(2, "Подтяжка груди (мастопексия)"),
                new KeyValuePair<int, string>(3, "Уменьшение груди (редукционная маммопластика)"),
                new KeyValuePair<int, string>(4, "Пластика век (блефаропластика)"),
                new KeyValuePair<int, string>(5, "Пластика передней брюшной стенки (абдоминопластика)"),
                new KeyValuePair<int, string>(6, "Пластика ушных раковин (отопластика)"),
                new KeyValuePair<int, string>(7, "Увеличение голеней (круропластика)"),
                new KeyValuePair<int, string>(8, "Уменьшение объёма жировой ткани (липосакция)"),
                new KeyValuePair<int, string>(9, "Пересадка волос (HFE аутотрансплантация)"),
                new KeyValuePair<int, string>(10, "Устранение гинекомастии (маскулинизирующая маммопластика)"),
                new KeyValuePair<int, string>(11, "Введение жировой ткани (липофилинг)"),
                new KeyValuePair<int, string>(12, "Протезирование полового члена (эндофаллопротезирование)"),
                new KeyValuePair<int, string>(13, "Подтяжка нижней трети лица (фейслифтинг)"),
                new KeyValuePair<int, string>(14, "Пластика носа (ринопластика)"),
                new KeyValuePair<int, string>(15, "Удаление комочков Биша"),
                new KeyValuePair<int, string>(16, "Маскулинизирующая маммопластика при транссексуализме"),
                new KeyValuePair<int, string>(17, "Пластика груди после мастэктомии"),
                new KeyValuePair<int, string>(18, "Пластика полового члена (фаллопластика)"),
                new KeyValuePair<int, string>(19, "Пластика дефектов мягких тканей"),
                new KeyValuePair<int, string>(20, "Хирургия кисти (удаление олеогранулёмы, устранение контрактуры Дюпюитрена)"),
                new KeyValuePair<int, string>(21, "Микрохирургическая аутотрансплантация пальцев стопы на кисть")
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
                _operationTypeTitles[0].Value,
                _operationTypeTitles[1].Value,
                _operationTypeTitles[2].Value,
                _operationTypeTitles[3].Value,
                _operationTypeTitles[4].Value,
                _operationTypeTitles[5].Value,
                _operationTypeTitles[6].Value,
                _operationTypeTitles[7].Value,
                _operationTypeTitles[8].Value,
                _operationTypeTitles[9].Value,
                _operationTypeTitles[14].Value,
                _operationTypeTitles[15].Value
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
                        .Where(x => Regex.IsMatch(x, FileExtensionsRegex, RegexOptions.IgnoreCase))
                        .Select(x => x.Replace(_hostingEnvironment.WebRootPath, ""));
                    var sampleThumbnails = Directory.GetFiles(sampleDirectory + "\\thumbnails")
                        .Where(x => Regex.IsMatch(x, FileExtensionsRegex, RegexOptions.IgnoreCase))
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

        public IActionResult Сomments()
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
