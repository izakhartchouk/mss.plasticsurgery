using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;
using MSS.PlasticSurgery.Models;

namespace MSS.PlasticSurgery.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<Operation, int> _operationRepository;

        public HomeController(IGenericRepository<Operation, int> operationRepository)
        {
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

        public IActionResult NewServices()
        {
            return View();
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
