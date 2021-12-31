﻿using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Models.DTO
{
    public class MyFolder : IItemClient
    {
        public int FilesInFolder { get; set; }
        public int FoldersInFolder { get; set; }
        public int SizeFolders { get; set; }
        public AttributesFolder AttributesFolder { get; set; }
        public string ItemNewPath { get; set; }
        public string ItemOldType { get; set; }
    }
}
