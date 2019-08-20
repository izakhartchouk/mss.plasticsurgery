using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;
using MSS.PlasticSurgery.Models;
using MSS.PlasticSurgery.Utilities;

namespace MSS.PlasticSurgery.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdministrationController : Controller
    {
        private readonly IGenericRepository<Operation, int> _operationRepository;
        private readonly IGenericRepository<Image, int> _imageRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdministrationController(
            IGenericRepository<Operation, int> operationRepository,
            IGenericRepository<Image, int> imageRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _operationRepository = operationRepository;
            _imageRepository = imageRepository;
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

            operationDto.Images = operation.Images.Safe()
                .Select(filePath => new Image()
                {
                    Operation = operationDto,
                    Path = filePath
                })
                .ToArray();

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

        [HttpPost]
        public IActionResult GetOperation([FromBody] string operationId)
        {
            var operationEntity = _operationRepository.GetById(int.Parse(operationId));

            var operationViewModel = new OperationViewModel()
            {
                Id = operationEntity.Id,
                Title = operationEntity.Title,
                Subtitle = operationEntity.Subtitle,
                Description = operationEntity.Description,
                Images = operationEntity.Images.Select(x => x.Path)
            };

            return Json(operationViewModel);
        }

        public IActionResult UpdateOperation(OperationViewModel operation)
        {
            var operationEntity = new Operation()
            {
                Id = operation.Id,
                Title = operation.Title,
                Subtitle = operation.Subtitle,
                Description = operation.Description,
            };

            var imageEntities = operation.Images.Safe()
                .Select(filePath => new Image()
                {
                    OperationId = operation.Id,
                    Path = filePath
                });

            foreach (var imageEntity in imageEntities)
            {
                _imageRepository.Create(imageEntity);
            }

            _operationRepository.Update(operationEntity);

            return Json("success");
        }

        [HttpPost]
        public IActionResult DeleteOperation([FromBody] string operationId)
        {
            var targetOperationId = int.Parse(operationId);
            Operation operationEntity = _operationRepository.GetById(targetOperationId);
            DeleteFiles(operationEntity.Images.Select(x => x.Path));
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

        [HttpPost]
        public IActionResult DeleteFiles(IEnumerable<string> filePaths, bool shouldPersist)
        {
            var filePathsArray = filePaths.ToArray();

            DeleteFiles(filePathsArray);

            if (shouldPersist)
            {
                var imageEntities =_imageRepository.Get(x => filePathsArray.Contains(x.Path));

                foreach (var imageEntity in imageEntities)
                {
                    _imageRepository.Delete(imageEntity);
                }
            }

            return Ok(new
            {
                message = "success"
            });
        }

        private void DeleteFiles(IEnumerable<string> relativeFilePaths)
        {
            foreach (var relativeFilePath in relativeFilePaths)
            {
                string serverFilePath = _hostingEnvironment.GenerateServerFilePath(relativeFilePath);

                if (System.IO.File.Exists(serverFilePath))
                {
                    System.IO.File.Delete(serverFilePath);
                }
            }
        }
    }
}