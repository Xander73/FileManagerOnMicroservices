using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Responses
{
    public class AllDrivesResponse
    {
        public List<string> Drives { get; set; }


        public AllDrivesResponse()
        {
            Drives = new List<string>();
        }
    }
}
