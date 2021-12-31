using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Item : IItem
    {
        public string Path { get; set; }

        public TypeItem IsFolderOrFile { get; set; }


    }
}
