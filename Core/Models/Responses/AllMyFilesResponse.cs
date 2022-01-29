using Core.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Responses
{
    public class AllMyFilesResponse
    {
        public List<MyFileDTO> Items { get; set; }
        public Exception ex;


        public AllMyFilesResponse()
        {
            Items = new List<MyFileDTO>();
            ex =new Exception();
        }
    }
}
