using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MSS.PlasticSurgery.Utilities
{
    public static class HostingEnvironmentExtensions
    {
        public static string GenerateServerFilePath(this IHostingEnvironment hostingEnvironment, string relativeFilePath)
        {
            return hostingEnvironment.WebRootPath + relativeFilePath.Replace("/", "\\");
        }

        public static string GetFilePathForStoring(this IHostingEnvironment hostingEnvironment, IFormFile file, out string relativeFilePath)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var randomFileName = Path.GetRandomFileName();
            randomFileName = Path.ChangeExtension(randomFileName, fileExtension);
            relativeFilePath = $"/img/operations/{randomFileName}";

            return Path.Combine(hostingEnvironment.WebRootPath, "img\\operations", randomFileName);
        }
    }
}