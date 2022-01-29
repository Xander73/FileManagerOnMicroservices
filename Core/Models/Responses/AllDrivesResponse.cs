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
        public Exception ex;


        public AllDrivesResponse()
        {
            Drives = new List<string>();
            ex = new Exception();
        }
    }
}
