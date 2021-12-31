
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public enum TypeItem { NotExists, Folder, File}

    public interface IItem
    {
        string Path { get; set; }
        TypeItem IsFolderOrFile { get; set; }
    }
}
