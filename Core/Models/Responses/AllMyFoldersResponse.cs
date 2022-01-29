using Core.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Responses
{
    public class AllMyFoldersResponse
    {
        public List<MyFolderDTO> Items { get; set; }
        public Exception ex;


        public AllMyFoldersResponse()
        {
            Items = new List<MyFolderDTO>();
            ex = new Exception();
        }
    }
}
