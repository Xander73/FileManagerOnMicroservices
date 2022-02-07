using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Requests
{
    public class CopyItemRequest
    {
        public string ClientBaseAddres { get; set; }
        public string NewPath { get; set; }
        public string OldPath { get; set; }
    }
}
