using Core.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Core.Tests
{
    public class MyFileTests
    {
        MyFile myFile;
        string testPathDirectory = Directory.GetCurrentDirectory() + "\\testDirectory";
        string testPathFile = "";


        public MyFileTests()
        {
            Directory.CreateDirectory(testPathDirectory);
            testPathFile = testPathDirectory + "\\test.txt";
            myFile = new MyFile(testPathFile);
            myFile.CreateFile(testPathFile);
            //using (FileStream fs = new FileStream(testPathFile, FileMode.OpenOrCreate));

            string text = "1111 111\n\t22 222";
            using (StreamWriter sr = new StreamWriter(testPathFile))
            {
                sr.WriteLine(text);
            }
        }

        
        [Fact]
        public void CreateFile_CurrentDirAndNewFileName_TrueReturned()
        {
            string filePath = testPathDirectory + "\\CreateFile.txt";
            myFile.CreateFile(filePath);

            Assert.True(File.Exists(filePath));
        }


        [Fact]
        public void CreateFile_CurrentDirAndNewFileName_FileExist_TrueReturned()
        {
            string filePath = testPathDirectory + "\\CreateFile.txt";
            myFile.CreateFile(filePath);

            Assert.True(File.Exists(filePath));
        }


        [Fact]
        public void Delete_CurrentDir_TrueReturned()
        {
            string filePath = testPathDirectory + "\\DeleteFile.txt";
            MyFile myFileTemp = new MyFile(filePath);
            myFileTemp.CreateFile(filePath);
            Assert.True(File.Exists(myFileTemp.FullPath));
            myFileTemp.DeleteFile(filePath);
            Assert.False(File.Exists(filePath));
        }


        [Fact]
        public void RenameFile_CurrentDir_TrueReturned()
        {
            string oldFilePath = testPathDirectory + "\\FileToRename.txt";
            string newFilePath = testPathDirectory + "\\FileAfterRename";
            IMyFile myFileTemp = new MyFile(newFilePath);
            myFileTemp.CreateFile(newFilePath);
            myFileTemp.RenameFile(oldFilePath, newFilePath);

            Assert.True(File.Exists(newFilePath));
        }


        [Fact]
        public void CopyFile_CurrentDirAndCurrentDirTest_TrueReturned()
        {
            string filePathBeforeCopy = testPathDirectory + "\\FileToCopy.txt";
            MyFile myFileTemp = new MyFile(filePathBeforeCopy);
            myFileTemp.CreateFile(filePathBeforeCopy);
            string directoryToCopy = testPathDirectory + "\\DirectoryToCopyFile";
            string filePathAfterCopy = directoryToCopy + "\\FileToCopy.txt";
            Directory.CreateDirectory(directoryToCopy);
            myFileTemp.CopyFile(filePathBeforeCopy, filePathAfterCopy);

            Assert.True(File.Exists(filePathAfterCopy));
        }


        [Fact]
        public void SizeFile_TestFile_18ByteReturned()
        {
            string filePathSize = testPathDirectory + "\\FileToSize.txt";
            MyFile myFileTemp = new MyFile(filePathSize);
            myFileTemp.CreateFile(filePathSize);

            string text = "";
            for(int i = 0; i < 10_000; ++i)
            {
                text += "111111111";
            }
            using (StreamWriter sr = new StreamWriter(filePathSize))
            {
                sr.WriteLine(text);
            }
            long expected = 90;
            long actual = myFileTemp.SizeFile();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetFileAttributes_NotTxtFile_ArchiveReturned()
        {
            string filePath = testPathDirectory + "\\FileToAttributes.doc";
            MyFile myFileTemp = new MyFile(filePath);
            myFile.CreateFile(filePath);

            string execute = "Archive";

            string actual = myFile.GetFileAttributes();

            Assert.Equal(execute, actual);
        }


        [Fact]
        public void SetFileAttributes_Hiden_HidenReturned()
        {
            string filePath = testPathDirectory + "\\HiddenFileAttribute.txt";
            MyFile myFileTemp = new MyFile(filePath);
            if (File.Exists(filePath))
            {
                myFileTemp.SetFileAttributes(FileAttributes.Archive);
            }
            else
            {
                myFileTemp.CreateFile(filePath);
            }            

            myFileTemp.SetFileAttributes(FileAttributes.Hidden);

            string execute = "Hidden";

            string actual = myFileTemp.GetFileAttributes();

            myFileTemp.SetFileAttributes(FileAttributes.Archive);

            Assert.Equal(execute, actual);
        }


        [Fact]
        public void GetFileAttributes_TxtFile_ArchiveAttributeReturned()
        {
            //string execute = "Paragraphes - 1\nWords - 4\nChars - 15\nChars without space - 13\nArchive";

            string execute = "Archive";

            string actual = myFile.GetFileAttributes();

            Assert.Equal(execute, actual);
        }


        [Fact]
        public void TextFileInformmation_TxtFile_TextFileAttributeReturned()
        {
            Dictionary<string, int> execute = new Dictionary<string, int> 
            {
                ["Paragraphes" ] = 1, 
                ["Words"] = 4, 
                ["Chars"] = 15, 
                ["CharsWithoutSpace"] = 13
            };

            Dictionary<string, int> actual = myFile.TextFileInformmation();
            Assert.True(Enumerable.SequenceEqual(execute, actual));
        }


        [Fact]
        public void IsTextFile_FileWithEndsTXT_TrueReturned()
        {            
            Assert.True(myFile.IsTextFile());
        }


        [Fact]
        public void IsTextFile_FileWithEndsDOC_FalseReturned()
        {
            string pathDocFile = testPathDirectory + "\\testFile.doc";
            MyFile testDocFile = new MyFile(pathDocFile);
            Assert.False(testDocFile.IsTextFile());
        }
    }
}
