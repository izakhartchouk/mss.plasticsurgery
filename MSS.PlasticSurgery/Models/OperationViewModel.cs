using System.Collections.Generic;

namespace MSS.PlasticSurgery.Models
{
    public class OperationViewModel
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Photos { get; set; }
    }
}
