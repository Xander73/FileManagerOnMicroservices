using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Models.Requests
{ 
    public class RenameItem
    {
        public string PathCurrentPath { get; set; }
        public string OldNameItem { get; set; }
        public string NewNameItem { get; set; }

    }
}
