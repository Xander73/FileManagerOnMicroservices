using Core.Interfaces;


namespace Core
{
    public class MyFolder : IMyFolder
    {
        public string CurrentPath { get; set; }
        public int FilesInFolder { get; set; }
        public int FoldersInFolder { get; set; }
        public int SizeFolders { get; set; } = 0;

        public MyFolder(string path)
        {
            CurrentPath = path;
            
        }


        public void CreateFolder()
        {
            Directory.CreateDirectory(CurrentPath + '\\' + "New Folder") ;
        }


        public void CreateFolder(string nameFolder)
        {
            Directory.CreateDirectory(CurrentPath + '\\' + nameFolder) ;
        }


        public void DeleteFolder(string name)
        {
            Directory.Delete(CurrentPath + '\\' + name);
        }


        public void RenameFolder(string oldName, string newName)
        {
            Directory.Move(CurrentPath + '\\' + oldName, CurrentPath + '\\' + newName);
        }


        public void CopyFolder(string folderName, string newPath)
        {
            foreach (string dirPath in Directory.GetDirectories(CurrentPath + '\\' + folderName, "*",
                        SearchOption.AllDirectories))
            {
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(CurrentPath + '\\' + folderName, newPath + '\\' + folderName));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            //Копировать все файлы и перезаписать файлы с идентичным именем
            foreach (string newFilePath in Directory.GetFiles(CurrentPath + '\\' + folderName, "*.*",
                        SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newFilePath, newFilePath.Replace(CurrentPath + '\\' + folderName, newPath), true);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        public int SizeFolder(string name)
        {
            string[] files = Directory.GetFiles(name);
            FilesInFolder += files.Length;
            foreach (string file in files)
            {
                SizeFolders +=  File.ReadAllBytes(file).Length;
            }

            string[] folders = Directory.GetDirectories(name);
            FoldersInFolder += folders.Length;

            foreach (string folder in folders)
            {
                SizeFolder(folder);
            }
            return SizeFolders;
        }


        public List<string> Search(string path, string nameSearch)
        {
            List<string> results = new List<string>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (nameSearch == Path.GetDirectoryName(file))
                {
                    results.Add(file);
                }
            }

            string[] folders = Directory.GetDirectories(path);

            foreach (string item in folders)
            {
                string str = Path.GetFileName(item);
                if (nameSearch == Path.GetFileName(item))
                {
                    results.Add(item);
                }
                results.AddRange(Search(item, nameSearch));
            }

            return results;
        }


        public IEnumerable<string> GetItemsInFolder()
        {
            string[] folders = Directory.GetDirectories(CurrentPath);
            string[] files = Directory.GetFiles(CurrentPath);

            return folders.Concat(files);
        }


        public string GetName(string fullPath)
        {
            int index = fullPath.LastIndexOf('\\');
            return fullPath.Substring(index, fullPath.Length - 1 - index);
        }
    }
}
