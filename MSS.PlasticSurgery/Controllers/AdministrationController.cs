using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.Models;

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
                    Photos = new string[]
                    {
                        _hostingEnvironment.WebRootFileProvider.GetFileInfo("img/operations/op-1.png")?.PhysicalPath,
                        _hostingEnvironment.WebRootFileProvider.GetFileInfo("img/operations/op-2.png")?.PhysicalPath
                    }
                },
                new OperationViewModel()
                {
                    Title = "Operation 2",
                    Subtitle = "Operation Subtitle 2",
                    Description = "Operation Description 2",
                    Photos = new string[]
                    {
                        _hostingEnvironment.WebRootFileProvider.GetFileInfo("img/operations/op-1.png")?.PhysicalPath,
                        _hostingEnvironment.WebRootFileProvider.GetFileInfo("img/operations/op-2.png")?.PhysicalPath
                    }
                },
                new OperationViewModel()
                {
                    Title = "Operation 3",
                    Subtitle = "Operation Subtitle 3",
                    Description = "Operation Description 3",
                    Photos = new string[]
                    {
                        _hostingEnvironment.WebRootFileProvider.GetFileInfo("img/operations/op-1.png")?.PhysicalPath,
                        _hostingEnvironment.WebRootFileProvider.GetFileInfo("img/operations/op-2.png")?.PhysicalPath
                    }
                }
            };
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
