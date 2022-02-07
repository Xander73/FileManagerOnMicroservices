
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public enum TypeItem { NotExists, Folder, File, TextFile}


    public interface IItem
    {
        string Name { get; set; }
        string FullPath { get; set; }
        TypeItem FolderOrFile { get; set; }
    }
}
