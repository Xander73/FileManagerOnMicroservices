

namespace Core.Interfaces
{
    public interface IMyFile
    {
        void CreateFile(string name);


        void DeleteFile(string pathDelete);


        void RenameFile(string oldFile, string newFile);


        MyFile CopyFile(string newFullPath, string newName);


        long SizeFile();


        public string GetFileAttributes();


        public string SetFileAttributes(FileAttributes attributes);


        Dictionary<string, int> TextFileInformmation();
    }
}
