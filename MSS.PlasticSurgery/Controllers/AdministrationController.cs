using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.Models;
using System.Collections.Generic;

namespace MSS.PlasticSurgery.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private List<OperationViewModel> operationViewModels;

        public AdministrationController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            operationViewModels = new List<OperationViewModel>()
            {
                new OperationViewModel()
                {
                    Title = "Operation 1",
                    Subtitle = "Operation Subtitle 1",
                    Description = "Operation Description 1",
                    Images = new string[]
                    {
                        "~/img/operations/op-1.jpg",
                        "~/img/operations/op-2.jpg"
                    }
                },
                new OperationViewModel()
                {
                    Title = "Operation 2",
                    Subtitle = "Operation Subtitle 2",
                    Description = "Operation Description 2",
                    Images = new string[]
                    {
                        "~/img/operations/op-1.jpg",
                        "~/img/operations/op-2.jpg"
                    }
                },
                new OperationViewModel()
                {
                    Title = "Operation 3",
                    Subtitle = "Operation Subtitle 3",
                    Description = "Operation Description 3",
                    Images = new string[]
                    {
                        "~/img/operations/op-1.jpg",
                        "~/img/operations/op-2.jpg"
                    }
                }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetOperations()
        {
            return PartialView("Shared/_OperationsTabPartial", operationViewModels);
        }
    }
}