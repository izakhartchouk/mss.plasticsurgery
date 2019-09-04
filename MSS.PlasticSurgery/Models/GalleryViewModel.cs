using System.Collections.Generic;

namespace MSS.PlasticSurgery.Models
{
    public class GalleryViewModel
    {
        public string[] OperationTypeTitles { get; set; }

        public Dictionary<string, string> ImagesAndThumbnails { get; set; }
    }
}
