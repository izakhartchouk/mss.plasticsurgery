using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;
using MSS.PlasticSurgery.Models;

namespace MSS.PlasticSurgery.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IGenericRepository<Operation, int> _operationRepository;

        public AdministrationController(
            IGenericRepository<Operation, int> operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetOperations()
        {
            var operationViewModels = _operationRepository.GetAll()
                .Select(x => new OperationViewModel()
                {
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    Description = x.Description,
                    Images = x.Images.Select(y => y.Path)
                });

            return PartialView("Shared/_OperationsTabPartial", operationViewModels);
        }
    }
}