

namespace Core.Interfaces
{
    public interface IMyFolder
    {
        void CreateFolder();


        void CreateFolder(string nameFolder);


        void DeleteFolder(string fullName);


        void RenameFolder(string oldName, string newName);


        void CopyFolder(string oldName, string newName);


        int SizeFolder(string name);


        List<string> Search(string path, string name);


        IEnumerable<string> GetItemsInFolder();
    }
}
