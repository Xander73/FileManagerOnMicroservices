using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Requests
{
    public class SearchItemRequest
    {
        public string ClientBaseAddres { get; set; }
        public string PathFolder { get; set; }
        public string SearchNameItem { get; set; }
    }
}
