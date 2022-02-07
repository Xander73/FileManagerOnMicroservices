

namespace Core.Interfaces
{
    public interface IMyFolder
    {
        void CreateFolder();


        void CreateFolder(string nameFolder);


        void DeleteFolder(string fullName);


        void RenameFolder(string oldName, string newName);


        void CopyFolder(string oldName, string newName);


        void SizeFolder(string name);


        static List<Item> Search(string path, string name) => new List<Item>();
    }
}
