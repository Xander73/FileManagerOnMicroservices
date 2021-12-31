

namespace Core.Interfaces
{
    public interface IMyFile
    {
        string Name { get; set; }

        string FilePath { get; set; }


        string NameToPath();


        void CreateFile();


        void CreateFile(string name);


        void DeleteFile();


        void RenameFile(string newName);


        MyFile CopyFile(string newFullPath, string newName);


        long SizeFile();


        public string GetFileAttributes();


        public string SetFileAttributes(FileAttributes attributes);


        string TextFileInformmation();
    }
}
