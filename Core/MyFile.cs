using Core.Interfaces;

namespace Core
{
    public class MyFile : Item, IMyFile
    {
        public MyFile(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);
        }


        public string NameToPath() => '\\' + Name;

        public void CreateFile()
        {
            using (File.Create(FullPath + '\\' + "New File")) { };
            Name = "New File";
        }


        public void CreateFile(string pathNewFile)
        {
            File.Create(pathNewFile);
        }


        public void DeleteFile(string pathDelete)
        {
            File.Delete(pathDelete);
        }


        public void RenameFile(string oldFile, string newName)
        {            
            try
            {
                File.Move(oldFile, newName);
            }
            catch (Exception e)
            {
                string s = e.Message;
            }

            Name = Path.GetFileName(newName);
        }


        public MyFile CopyFile(string pathOldFile, string pathNewFile)
        {
            File.Copy(pathOldFile, GetNewNameWithSuffixNumberIfIsEqualsFiles(pathNewFile));

            return new MyFile(pathNewFile);
        }


        public long SizeFile()
        {
            return new FileInfo(FullPath).Length / 1000;
        }

        public string GetFileAttributes()
        {
            string attributes = "";
            return attributes += File.GetAttributes(FullPath + NameToPath()).ToString();
        }

        public string SetFileAttributes(FileAttributes attributes)
        {
            File.SetAttributes(FullPath + NameToPath(), FileAttributes.Normal);
            File.SetAttributes(FullPath + NameToPath(), attributes);
            return GetFileAttributes(); ;
        }



        public Dictionary<string, int> TextFileInformmation()
        {
            if (IsTextFile())
            {
                Dictionary<string, int> textAttributes = new Dictionary<string, int>();
                List<string> stringsFromFile = File.ReadAllLines(FullPath).ToList();

                textAttributes["Paragraphes"] = GetParagraph(stringsFromFile);

                textAttributes["Words"] = GetWords(stringsFromFile);

                textAttributes["Chars"] = GetChars(stringsFromFile);

                textAttributes["CharsWithoutSpace"] = GetCharsWithoutSpace(stringsFromFile);

                return textAttributes;
            }
            else return new Dictionary<string, int>();
        }


        public bool IsTextFile () => Name.EndsWith(".txt");


        public int GetParagraph(List<string> stringFromFile)
        {
            int paragraphs = 0;
            int spases;
            const int SPACES_IN_START_PARAGRAPH = 4;

            foreach (string item in stringFromFile)
            {
                spases = 0;
                if (item[0] == ' ')
                {
                    for (int i = 0; i < item.Length || item[i] != ' '; i++)
                    {
                        ++spases;
                    }
                }

                if (spases >= SPACES_IN_START_PARAGRAPH || item[0] == '\t')
                {
                    ++paragraphs;
                }
            }
            return paragraphs;
        }


        public int GetWords(List<string> stringFromFile)
        {
            int words = 0;
            foreach (string item in stringFromFile)
            {
                string[] temp = item.Split(' ');
                words += temp.Length;
            }
            return words;
        }


        public int GetChars(List<string> stringFromFile)
        {
            int chars = 0;
            foreach (string item in stringFromFile)
            {
                chars += item.Length;
            }

            return chars;
        }


        public int GetCharsWithoutSpace(List<string> stringFromFile)
        {
            int charsWithoutSpases = 0;
            foreach (string item in stringFromFile)
            {
                charsWithoutSpases += item.Replace(" ", "").Length;
            }
            return charsWithoutSpases;
        }


        private string GetNewNameWithSuffixNumberIfIsEqualsFiles(string pathCheckedItem)
        {
            if (File.Exists(pathCheckedItem))
            {
                string parentFolder = Directory.GetParent(pathCheckedItem).FullName;
                string name = Path.GetFileNameWithoutExtension(pathCheckedItem);
                string extension = Path.GetExtension(pathCheckedItem);
                int suffixNewFile = 1;
                while (File.Exists(
                    parentFolder + '\\' + name 
                    + suffixNewFile.ToString() 
                    + extension))
                {
                    suffixNewFile++;
                }
                return parentFolder + '\\' + name
                    + suffixNewFile.ToString()
                    + extension;
            }

            return pathCheckedItem;
        }
    }
}
