using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;
using MSS.PlasticSurgery.Models;

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
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    Description = x.Description,
                    Images = x.Images.Select(y => y.Path)
                });

            return PartialView("Shared/_OperationsTabPartial", operationViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var randomFileName = Path.GetRandomFileName();
            randomFileName = Path.ChangeExtension(randomFileName, fileExtension);
            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "img\\operations", randomFileName);
            var relativePath = $"/img/operations/{randomFileName}";

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