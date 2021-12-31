using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Models.Requests
{
    public class CopyItem
    {
        public string NameItem { get; set; }
        public string NewPath { get; set; }
        public string OldPath { get; set; }
    }
}
