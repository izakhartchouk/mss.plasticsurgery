using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;
using MSS.PlasticSurgery.Models;
using MSS.PlasticSurgery.Utilities;

namespace MSS.PlasticSurgery.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IGenericRepository<Operation, int> _operationRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdministrationController(
            IGenericRepository<Operation, int> operationRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _operationRepository = operationRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOperation(OperationViewModel operation)
        {
            var operationDto = new Operation()
            {
                Title = operation.Title,
                Subtitle = operation.Subtitle,
                Description = operation.Description
            };

            operationDto.Images = operation.Images.Select(filePath => new Image()
            {
                Operation = operationDto,
                Path = filePath
            }).ToArray();

            _operationRepository.Create(operationDto);

            return Json("success");
        }

        public IActionResult GetOperations()
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

            return PartialView("Shared/_OperationsTabPartial", operationViewModels);
        }

        public IActionResult UpdateOperation(OperationViewModel operation)
        {
            return Json("success");
        }

        [HttpPost]
        public IActionResult DeleteOperation([FromBody] string operationId)
        {
            var targetOperationId = int.Parse(operationId);

            Operation operationEntity = _operationRepository.GetById(targetOperationId);

            foreach (var relativeFilePath in operationEntity.Images.Select(x => x.Path))
            {
                string serverFilePath = _hostingEnvironment.GenerateServerFilePath(relativeFilePath);

                if (System.IO.File.Exists(serverFilePath))
                {
                    System.IO.File.Delete(serverFilePath);
                }
            }

            _operationRepository.Delete(targetOperationId);

            return Json("success");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var uploadPath = _hostingEnvironment.GetFilePathForStoring(file, out var relativePath);

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return Ok(new
            {
                size = file.Length,
                filePath = relativePath
            });
        }
    }
}