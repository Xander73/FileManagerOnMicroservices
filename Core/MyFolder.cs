using Core.Interfaces;


namespace Core
{
    public class MyFolder : Item, IMyFolder
    {
        public int FilesInFolder { get; set; }
        public int FoldersInFolder { get; set; }
        public long SizeFolders { get; set; } = 0;

        public MyFolder(string path)
        {
            FullPath = path;
            Name = Path.GetFileName(path);
        }


        public void CreateFolder()
        {
            Directory.CreateDirectory(FullPath + '\\' + "New Folder") ;
        }


        public void CreateFolder(string pathNewFolder)
        {
            Directory.CreateDirectory(pathNewFolder);
        }


        public void DeleteFolder(string pathDelete)
        {
            Directory.Delete(pathDelete, true);
        }


        public void RenameFolder(string oldName, string newName)
        {
            Directory.Move(oldName, newName);
        }

        
        public void CopyFolder(string pathOldFolder, string pathNewFolder)
        {
            Directory.CreateDirectory(pathNewFolder);
            foreach (string dirPath in Directory.GetDirectories(pathOldFolder, "*",
                        SearchOption.AllDirectories))
            {
                string s = dirPath.Replace(pathOldFolder, pathNewFolder);
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(pathOldFolder, pathNewFolder));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            foreach (string newFilePath in Directory.GetFiles(pathOldFolder, "*.*",
                        SearchOption.AllDirectories))
            {
                try
                {
                    string s = newFilePath.Replace(pathOldFolder, pathNewFolder);
                    File.Copy(newFilePath, newFilePath.Replace(pathOldFolder, pathNewFolder), true);
                }
                catch (Exception e)
                {
                    string s = e.Message;
                }
            }
        }


        public void SizeFolder(string name)
        {
            if (new DirectoryInfo(name).Attributes.HasFlag(FileAttributes.System)) 
            {
                return;
            }
            try
            {
                string[] files = Directory.GetFiles(name);
                FilesInFolder += files.Length;
                foreach (string file in files)
                {
                    SizeFolders += new FileInfo(file).Length;
                }

                string[] folders = Directory.GetDirectories(name);
                FoldersInFolder += folders.Length;

                foreach (string folder in folders)
                {
                    if (!new DirectoryInfo(folder).Attributes.HasFlag(FileAttributes.System))
                    {
                        SizeFolder(folder);
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                return;
            }
        }


        public static List<Item> Search(string path, string nameSearch)
        {
            List<Item> results = new List<Item>();

            results.AddRange(SearchFolderInCurrentFolder(path, nameSearch));
            results.AddRange(SearchFilesInCurrentFolder(path, nameSearch));
            return results;
        }


        public static List<Item> SearchFolderInCurrentFolder(string path, string nameSearch)
        {
            List<Item> results = new List<Item>();
            try
            {                
                string[] folders = Directory.GetDirectories(path);
                foreach (string item in folders)
                {
                    string str = Path.GetFileName(item);
                    if (nameSearch == Path.GetFileNameWithoutExtension(item))
                    {
                        results.Add(new Item()
                        {
                            Name = Path.GetFileName(item),
                            FullPath = item,
                            FolderOrFile = TypeItem.Folder
                        });
                    }
                    results.AddRange(Search(item, nameSearch));
                }
            }
            catch (Exception e)
            {

                string s = e.Message;
            }
            
            return results;
        }


        public static List<Item> SearchFilesInCurrentFolder(string path, string nameSearch)
        {
            List<Item> results = new List<Item>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (nameSearch == Path.GetFileNameWithoutExtension(file))
                {
                    results.Add(new Item()
                    {
                        Name = Path.GetFileName(file),
                        FullPath = file,
                        FolderOrFile = TypeItem.File
                    });
                }
            }
            return results;
        }


        public IEnumerable<string> GetItemsInFolder()
        {
            string[] folders = Directory.GetDirectories(FullPath);
            string[] files = Directory.GetFiles(FullPath);

            return folders.Concat(files);
        }
    }
}
