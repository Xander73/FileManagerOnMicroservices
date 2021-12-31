using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Models
{
    public class AttributesFolder
    {
        public int Size { get; set; }
        public bool Hidden { get; set; }
        public bool ReadOnly { get; set; }
        public int FilesInFolder { get; set; }
        public int FoldersInFolder { get; set; }
    }
}
