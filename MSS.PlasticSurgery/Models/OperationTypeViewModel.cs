using System.Collections.Generic;

namespace MSS.PlasticSurgery.Models
{
    public class OperationTypeViewModel
    {
        public string Title { get; set; }

        public IEnumerable<Dictionary<string, string>> Samples { get; set; }
    }
}
