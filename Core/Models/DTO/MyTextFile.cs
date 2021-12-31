using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Models.DTO
{
    public class MyTextFile : IItemClient
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public AttributesTextFile Attributes { get; set; }
        public string ItemNewPath { get; set; }
        public string ItemOldType { get; set; }
    }
}
