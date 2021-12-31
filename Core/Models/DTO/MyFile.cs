using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Models.DTO
{
    public class MyFile : IItemClient
    {
        public AttributesFile Attributes { get; set; }
        public string ItemNewPath { get; set; }
        public string ItemOldType { get; set; }
    }
}
