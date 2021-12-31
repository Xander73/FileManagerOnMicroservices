using Core.Interfaces;

namespace Core
{
    public class MyFile : IMyFile
    {
        public string Name { get ; set; }
        public string FilePath { get; set; }


        public MyFile(string fullPath)
        {
            FilePath = fullPath;
            Name = Path.GetFileName(fullPath);
        }


        public string NameToPath() => '\\' + Name;

        public void CreateFile()
        {
            using (File.Create(FilePath + '\\' + "New File")) { };
            Name = "New File";
        }


        public void CreateFile(string name)
        {
            using (File.Create(FilePath + '\\' + name)) 
            Name = name;
        }


        public void DeleteFile()
        {
            File.Delete(FilePath + NameToPath());
        }


        public void RenameFile(string newName)
        {
            string suffux = GetSuffixNumberIfIsEqualsFiles(FilePath, newName);
            File.Move(FilePath + "\\" + Name, FilePath + "\\" + newName
                + GetSuffixNumberIfIsEqualsFiles(FilePath, newName));

            Name = newName;
        }


        public MyFile CopyFile(string newFullPath, string newName)
        {
            File.Copy(FilePath + NameToPath(), newFullPath + "\\" + newName 
                + GetSuffixNumberIfIsEqualsFiles(FilePath, newName));

            return new MyFile(newFullPath);
        }


        public long SizeFile()
        {
            return new FileInfo(FilePath + NameToPath()).Length;
        }

        public string GetFileAttributes()
        {
            string attributes = "";
            if (Name.EndsWith(".txt"))
            {
                attributes = TextFileInformmation();
            }
            return attributes += File.GetAttributes(FilePath + NameToPath()).ToString(); ;
        }

        public string SetFileAttributes(FileAttributes attributes)
        {
            File.SetAttributes(FilePath + NameToPath(), FileAttributes.Normal);
            File.SetAttributes(FilePath + NameToPath(), attributes);
            return GetFileAttributes(); ;
        }


        public string TextFileInformmation()
        {
            if (Name.EndsWith(".txt"))
            {
                string textFileInformation = "";
                List<string> stringsFromFile = File.ReadAllLines(FilePath + NameToPath()).ToList();

                textFileInformation += "Paragraphes - " + GetParagraph(stringsFromFile).ToString();

                textFileInformation += "\nWords - " + GetWords(stringsFromFile).ToString();

                textFileInformation += "\nChars - " + GetChars(stringsFromFile).ToString();

                textFileInformation += "\nChars without space - " + GetCharsWithoutSpace(stringsFromFile).ToString() + '\n';

                return textFileInformation;
            }
            else return String.Empty;
        }


        private int GetParagraph(List<string> stringFromFile)
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


        private int GetWords(List<string> stringFromFile)
        {
            int words = 0;
            foreach (string item in stringFromFile)
            {
                string[] temp = item.Split(' ');
                words += temp.Length;
            }
            return words;
        }


        private int GetChars(List<string> stringFromFile)
        {
            int chars = 0;
            foreach (string item in stringFromFile)
            {
                chars += item.Length;
            }

            return chars;
        }


        private int GetCharsWithoutSpace(List<string> stringFromFile)
        {
            int charsWithoutSpases = 0;
            foreach (string item in stringFromFile)
            {
                charsWithoutSpases += item.Replace(" ", "").Length;
            }
            return charsWithoutSpases;
        }


        private string GetSuffixNumberIfIsEqualsFiles(string path, string name)
        {

            if (File.Exists(path + "\\" + name))
            {
                int suffixNewFile = 1;
                while (File.Exists(path + "\\" + name + suffixNewFile.ToString()))
                {
                    suffixNewFile++;
                }
                return suffixNewFile.ToString();
            }

            return String.Empty;
        }
    }


    
}
